using Infrastructure.EntityFramework;
using Infrastructure.Repositories.CustomerRepository;
using Infrastructure.Repositories.MaterialRepository;
using NeApplication.IRepositoryies;
using NeApplication.IRepositoryies.Materials;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        readonly NovinDbContext NovinDb = new();

        private ICustomerManager _customerManager;

        private IMaterialManager _materialManager;

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
        
        public IMaterialManager materialManager
        {
            get
            {
                if (_materialManager == null)
                {
                    _materialManager = new MaterialManager(NovinDb);
                }
                return _materialManager;
            }
        }

        public void Dispose()
        {
            NovinDb.Dispose();
        }
    }
}
