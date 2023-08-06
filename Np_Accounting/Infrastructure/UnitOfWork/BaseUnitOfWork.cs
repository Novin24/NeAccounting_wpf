using Infrastructure.EntityFramework;

namespace Infrastructure.UnitOfWork
{
    public class BaseUnitOfWork : IDisposable
    {
        readonly BaseDomainDbContext BaseNovin = new BaseDomainDbContext();

        public void Dispose()
        {
        }
    }
}
