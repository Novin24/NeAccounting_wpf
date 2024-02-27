using Domain.Enities.NovinEntity.Remittances;
using Domain.NovinEntity.Customers;
using Domain.NovinEntity.Documents;
using DomainShared.Constants;
using DomainShared.Enums;
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

        public async Task<(string error, bool isSuccess)> CreateDocument(Guid customerId,
            long price,
            DocumntType type,
            PaymentType payType,
            string? descripion,
            DateTime submitDate,
            bool receivedOrPaid)
        {
            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, type, payType, descripion, submitDate, receivedOrPaid));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        #region Invoice(CRUD)
        public async Task<(string error, bool isSuccess)> CreateSellDocument(Guid customerId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
            List<RemittanceListViewModel> remittances)
        {
            List<SellRemittance> list = remittances.Select(t => new SellRemittance(t.MaterialId, t.AmountOf, t.Price, t.TotalPrice, submitDate, t.Description)).ToList();

            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.SellInv, PaymentType.Other, descripion, submitDate, false)
                .AddSellRemittance(list));
                if (commission != null && commission != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, (long)(price * (commission.Value / 100)), DocumntType.PayCom,
                        PaymentType.Other,$" پورسانت فاکتور ( {t.Entity.Serial} )",submitDate,true,(byte)commission.Value)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                }
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> CreateBuyDocument(Guid customerId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
            List<RemittanceListViewModel> remittances)
        {
            List<BuyRemittance> list = remittances.Select(t => new BuyRemittance(t.MaterialId, t.AmountOf, t.Price, t.TotalPrice, submitDate, descripion)).ToList();

            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.BuyInv, PaymentType.Other, descripion, submitDate, true)
                .AddBuyRemittance(list));
                if (commission != null && commission != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, (long)(price * (commission.Value / 100)), DocumntType.RecCom, PaymentType.Other,$" پورسانت فاکتور( {t.Entity.Serial} )",submitDate,false,(byte)commission.Value)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                };
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> CreatePayDocument(Guid customerId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate)
        {
            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.PayDoc, paymentType, descripion, submitDate, false));

                if (discount != null && discount != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, discount.Value, DocumntType.RecCom, PaymentType.Other,$" تخفیف فاکتور( {t.Entity.Serial} )",submitDate,false)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                };
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> CreateRecDocument(Guid customerId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate)
        {

            try
            {
                var t = await Entities.AddAsync(new Document(customerId, price, DocumntType.RecDoc, paymentType, descripion, submitDate, true));

                if (discount != null && discount != 0)
                {
                    await DbContext.SaveChangesAsync();
                    var comDoc = new List<Document>()
                    {
                        new (customerId, discount.Value, DocumntType.PayDiscount, PaymentType.Other,$" تخفیف فاکتور( {t.Entity.Serial} )",submitDate,true)
                    };
                    t.Entity.AddDocument(comDoc);
                    Entities.Update(t.Entity);
                };
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
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
        public async Task<PagedResulViewModel<InvoiceListDto>> GetInvoicesByDate(DateTime startTime,
            DateTime endTime,
            string desc,
            Guid cusId,
            bool leftOver,
            bool ignorePagination,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            int i = 1;
            PersianCalendar pc = new();
            List<InvoiceListDto> Remittances = [];
            var MyDoc = await TableNoTracking
                .Where(st => st.SubmitDate > startTime)
                .Where(et => et.SubmitDate < endTime)
                .Where(et => string.IsNullOrEmpty(desc) || et.Description.Contains(desc))
                .Where(p => p.CustomerId == cusId)
                .Select(t => new InvoiceDto()
                {
                    Id = t.Id,
                    Type = t.Type,
                    Date = t.SubmitDate,
                    Serial = t.Serial,
                    Description = t.Description,
                    Price = t.Price,
                    ReceivedOrPaid = t.IsReceived
                }).OrderBy(p => p.Date)
                .ToListAsync();


            if (leftOver)
            {
                InvoiceListDto rem = new()
                {
                    Row = 0,
                    Date = startTime,
                    IsDeletable = false,
                    IsEditable = false,
                    ShamsiDate = startTime.ToShamsiDate(pc),
                    Description = "باقی مانده از قبل",
                    Bed = MyDoc.Where(p => p.Date < startTime && !p.ReceivedOrPaid).Sum(p => p.Price),
                    Bes = MyDoc.Where(p => p.Date < startTime && p.ReceivedOrPaid).Sum(p => p.Price),
                };
                Remittances.Add(rem);
            }

            Remittances.AddRange(MyDoc.Where(p => !p.ReceivedOrPaid && p.Date >= startTime).Select(t => new InvoiceListDto
            {
                Description = t.Description,
                Date = t.Date,
                Type = t.Type,
                Serial = t.Serial.ToString(),
                Id = t.Id,
                IsEditable = true,
                IsDeletable = true,
                ShamsiDate = t.Date.ToShamsiDate(pc),
                Bed = t.Price,
                Bes = 0,
            }).ToList());

            Remittances.AddRange(MyDoc.Where(p => p.ReceivedOrPaid && p.Date >= startTime).Select(t => new InvoiceListDto
            {
                Description = t.Description,
                Date = t.Date,
                Type = t.Type,
                Id = t.Id,
                Serial = t.Serial.ToString(),
                IsEditable = true,
                IsDeletable = true,
                ShamsiDate = t.Date.ToShamsiDate(pc),
                Bed = 0,
                Bes = t.Price,
            }).ToList());

            Remittances = [.. Remittances.OrderBy(t => t.Date)];

            foreach (var item in Remittances)
            {
                item.Row = i;
                long bed = Remittances.Where(p => p.Row <= i && p.Row >= 1).Sum(p => p.Bed);
                long bes = Remittances.Where(p => p.Row <= i && p.Row >= 1).Sum(p => p.Bes);

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

            var totalCount = Remittances.Count;

            if (!ignorePagination)
            {
                Remittances = Remittances.SkipLast(--pageNum * pageCount).TakeLast(pageCount).ToList();
            }

            return new PagedResulViewModel<InvoiceListDto>(totalCount, pageCount, Remittances);
        }

        public Task<IEnumerable<DetailRemittanceDto>> GetRemittancesByDate(DateTime StartTime, DateTime EndTime, Guid CusId, bool LeftOver, string Description)
        {
            throw new NotImplementedException();
        }


        public async Task<List<SummaryDoc>> GetSummaryDocs(Guid? CusId, DocumntType type)
        {
            PersianCalendar pc = new();
            var list = await (from doc in DbContext.Set<Document>()
                                               .AsNoTracking()
                                               .Where(t => t.Type == type)
                                               .Where(t => CusId == null || t.CustomerId == CusId)

                              join cus in DbContext.Set<Customer>()
                                                      on doc.CustomerId equals cus.Id

                              select new SummaryDoc()
                              {
                                  SubmitDate = doc.SubmitDate,
                                  Cus_Name = cus.Name,
                                  Price = doc.Price.ToString("N0"),
                              })
                      .OrderByDescending(c => c.SubmitDate)
                      .Take(10)
                      .ToListAsync();

            list.ForEach(c =>
            {
                c.ShamsiDate = c.SubmitDate.ToShamsiDate(pc);
            });
            return list;
        }
        #endregion
    }
}
