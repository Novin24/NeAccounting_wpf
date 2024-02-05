using Domain.NovinEntity.Documents;
using Infrastructure.EntityFramework;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class DocumentManager(NovinDbContext context) : Repository<Document>(context), IDocumentManager
    {

    }
}
