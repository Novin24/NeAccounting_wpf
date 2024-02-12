using Domain.Enities.NovinEntity.Remittances;
using Domain.NovinEntity.Documents;
using DomainShared.ViewModels.Document;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class DocumentManager(NovinDbContext context) : Repository<Document>(context), IDocumentManager
    {
        public async Task<(string error, bool isSuccess, string docSerial)> CreateDocument(Guid customerId,
            uint price,
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

        public async Task<(string error, bool isSuccess, string docSerial)> CreateSellDocument(Guid customerId, uint price, string? descripion, DateTime submitDate, bool receivedOrPaid, List<RemittanceListViewModel> remittances)
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

        public async Task<(string error, bool isSuccess, string docSerial)> CreateBuyDocument(Guid customerId, uint price, string? descripion, DateTime submitDate, bool receivedOrPaid, List<RemittanceListViewModel> remittances)
        {
            List<BuyRemittance> list = remittances.Select(t => new BuyRemittance(t.MaterialId, t.AmountOf, t.Price, t.TotalPrice, submitDate, descripion)).ToList();

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

        public async Task<string> GetLastDocumntNumber(DocumntType type)
        {
            return (await TableNoTracking.OrderByDescending(t => t.CreationTime).Where(t => t.Type == type).Select(c => c.Serial).FirstOrDefaultAsync()).ToString();
        }
    }
}
