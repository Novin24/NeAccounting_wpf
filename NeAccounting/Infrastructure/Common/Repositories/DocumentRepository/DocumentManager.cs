using Domain.NovinEntity.Documents;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class DocumentManager(NovinDbContext context) : Repository<Document>(context), IDocumentManager
    {
        public async Task<string> GetLastDocumntNumber(DocumntType type)
        {
            return (await TableNoTracking.OrderByDescending(t=> t.CreationTime).Select(c => c.Serial)
                .FirstOrDefaultAsync()).ToString();
        }
    }
}
