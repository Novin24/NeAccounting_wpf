using Domain.NovinEntity.Documents;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class DocumentManager(NovinDbContext context) : Repository<Document>(context), IDocumentManager
    {
        public async Task<(string error, bool isSuccess)> CreateDocument( Guid customerId, uint price, string descripion, DateTime submitDate, bool receivedOrPaid)
        {
            try
            {
                var t = await Entities.AddAsync(new Document(customerId,price,descripion,submitDate,receivedOrPaid));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public Task<(string error, bool isSuccess)> CreateSellDocument(Guid customerId, uint price, string descripion, DateTime submitDate, bool receivedOrPaid)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetLastDocumntNumber(DocumntType type)
        {
            return (await TableNoTracking.OrderByDescending(t=> t.CreationTime).Select(c => c.Serial)
                .FirstOrDefaultAsync()).ToString();
        }
    }
}
