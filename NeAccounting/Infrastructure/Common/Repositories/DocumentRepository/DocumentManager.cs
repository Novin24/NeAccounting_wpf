using Domain.Enities.NovinEntity.Remittances;
using Domain.NovinEntity.Documents;
using DomainShared.Constants;
using DomainShared.Extension;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.PagedResul;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;
using System.Globalization;

namespace Infrastructure.Repositories
{
    public class DocumentManager(NovinDbContext context) : Repository<Document>(context), IDocumentManager
    {
        public async Task<(string error, bool isSuccess, string docSerial)> CreateDocument(Guid customerId,
            long price,
            DocumntType type,
            string? descripion,
            DateTime submitDate,
            bool receivedOrPaid)
        {
            string serial;
            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, type, descripion, submitDate, receivedOrPaid));
                await DbContext.SaveChangesAsync();
                serial = t.Entity.Serial.ToString();
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false, string.Empty);
            }
            return new(string.Empty, true, serial);
        }

        #region Invoice
        public async Task<(string error, bool isSuccess, string docSerial)> CreateSellDocument(Guid customerId, long price, string? descripion, DateTime submitDate, bool receivedOrPaid, List<RemittanceListViewModel> remittances)
        {
            List<SellRemittance> list = remittances.Select(t => new SellRemittance(t.MaterialId, t.AmountOf, t.Price, t.TotalPrice, submitDate, descripion)).ToList();

            string serial;
            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.SellInv, descripion, submitDate, receivedOrPaid, list));
                await DbContext.SaveChangesAsync();
                serial = t.Entity.Serial.ToString();
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false, string.Empty);
            }
            return new(string.Empty, true, serial);
        }

        public async Task<(string error, bool isSuccess, string docSerial)> CreateBuyDocument(Guid customerId, long price, string? descripion, DateTime submitDate, bool receivedOrPaid, List<RemittanceListViewModel> remittances)
        {
            List<BuyRemittance> list = remittances.Select(t => new BuyRemittance(t.MaterialId, t.AmountOf, t.Price, t.TotalPrice, submitDate, descripion)).ToList();

            string serial;
            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.BuyInv, descripion, submitDate, receivedOrPaid, list));
                await DbContext.SaveChangesAsync();
                serial = t.Entity.Serial.ToString();
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false, string.Empty);
            }
            return new(string.Empty, true, serial);
        }

        public async Task<string> GetLastDocumntNumber(DocumntType type)
        {
            return (await TableNoTracking.OrderByDescending(t => t.CreationTime).Where(t => t.Type == type).Select(c => c.Serial).FirstOrDefaultAsync()).ToString();
        }
        #endregion

        #region Status
        public async Task<long> GetDebt(Guid customerId)
        {
            return await TableNoTracking.Where(p => !p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();

        }

        public async Task<long> GetCredit(Guid customerId)
        {
            return await TableNoTracking.Where(p => p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();
        }

        public async Task<(long, string)> GetStatus(Guid customerId)
        {
            var tal = await TableNoTracking.Where(p => !p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();

            var bed = await TableNoTracking.Where(p => p.IsReceived && p.CustomerId == customerId)
                .Select(p => p.Price).SumAsync();

            long res = tal - bed;
            if (res == 0)
            {
                return (0, "تسویه");
            }
            if (res > 0)
            {
                return (res, "بدهکار");
            }
            if (res < 0)
            {
                return (res, "طلبکار");
            }
            return new(0, "خطا");
        }
        #endregion

        #region report
        public async Task<PagedResulViewModel<InvoiceListDto>> GetInvoicesByDate(DateTime StartTime,
            DateTime EndTime,
            string desc,
            Guid CusId,
            bool LeftOver,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            int i = 1;
            PersianCalendar pc = new();
            List<InvoiceListDto> Remittances = [];
            var query = TableNoTracking
                .Where(st => st.SubmitDate > StartTime)
                .Where(et => et.SubmitDate < EndTime)
                .Where(et => string.IsNullOrEmpty(desc) || et.Description.Contains(desc))
                .Where(p => p.CustomerId == CusId)
                .Select(t => new InvoiceDto()
                {
                    Date = t.SubmitDate,
                    Serial = t.Serial.ToString(),
                    Description = t.Description,
                    Price = t.Price,
                    ReceivedOrPaid = t.IsReceived
                }).OrderBy(p => p.Date)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var MyDoc = await query
                .Skip(--pageNum * pageCount)
                .Take(pageCount)
                .ToListAsync();

            if (LeftOver)
            {

                InvoiceListDto rem = new()
                {
                    Row = 0,
                    Date = StartTime,
                    ShamsiDate = StartTime.ToShamsiDate(pc),
                    Description = "باقی مانده از قبل",
                    Bed = MyDoc.Where(p => p.Date < StartTime && !p.ReceivedOrPaid).Sum(p => p.Price),
                    Bes = MyDoc.Where(p => p.Date < StartTime && p.ReceivedOrPaid).Sum(p => p.Price),
                };
                Remittances.Add(rem);
            }

            Remittances.AddRange(MyDoc.Where(p => !p.ReceivedOrPaid && p.Date >= StartTime).Select(t => new InvoiceListDto
            {
                Description = $"( {t.Serial} )  {t.Description}",
                Date = t.Date,
                ShamsiDate = t.Date.ToShamsiDate(pc),
                Bed = t.Price,
                Bes = 0,
                Serial = t.Serial
            }).ToList());

            Remittances.AddRange(MyDoc.Where(p => p.ReceivedOrPaid && p.Date >= StartTime).Select(t => new InvoiceListDto
            {
                Description = $"( {t.Serial} )  {t.Description}",
                Date = t.Date,
                ShamsiDate = t.Date.ToShamsiDate(pc),
                Bed = 0,
                Bes = t.Price,
                Serial = t.Serial
            }).ToList());

            Remittances = [.. Remittances.OrderBy(t => t.Date)];

            foreach (var item in Remittances)
            {
                item.Row = i;
                long bed = Remittances.Where(p => p.Row <= i && p.Row >= 1).Sum(p => p.Bed);
                long bes = Remittances.Where(p => p.Row <= i && p.Row >= 1).Sum(p => p.Bes);

                //try
                //{
                //    item.ShamsiDate = item.Date.ToShortDateString();
                //}
                //catch
                //{
                //    item.ShamsiDate = "-";
                //}
                item.LeftOver = Math.Abs(bed - bes);
                if (bes > bed)
                {
                    item.Status = "طلبکار";
                }
                else if (bed > bes)
                {
                    item.Status = "بدهکار";
                }
                else
                {
                    item.Status = "تسویه";
                }
                i++;
            }

            return new PagedResulViewModel<InvoiceListDto>(totalCount, pageCount, Remittances);
        }

        public Task<IEnumerable<DetailRemittanceDto>> GetRemittancesByDate(DateTime StartTime, DateTime EndTime, Guid CusId, bool LeftOver, string Description)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
