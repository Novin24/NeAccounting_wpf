using Infrastructure.EntityFramework;
using Infrastructure.Common.BaseRepositories.Users;
using NeApplication.IRepositoryies;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        readonly NovinDbContext NovinDb = new();

        private ICustomerManager _customerManager;

        public ICustomerManager customerManager
        {
            get
            {
                if (_customerManager == null)
                {
                    _customerManager = new CustomerManager(NovinDb);
                }
                return _customerManager;
            }
        }

        public void Dispose()
        {
            NovinDb.Dispose();
        }
    }
}
