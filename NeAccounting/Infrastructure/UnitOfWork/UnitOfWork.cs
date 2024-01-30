using Infrastructure.EntityFramework;
using Infrastructure.Repositories;
using NeApplication.IRepositoryies;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        readonly NovinDbContext NovinDb = new();

        private ICustomerManager _customerManager;

        private IMaterialManager _materialManager;

        private IWorkerManager _workerManager;

        private IFunctionManager _functionManager;

        private IAidManager _aidManager;

        private IUnitManager _unitManager;

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

        public IUnitManager unitManager
        {
            get
            {
                if (_unitManager == null)
                {
                    _unitManager = new UnitManager(NovinDb);
                }
                return _unitManager;
            }
        }

        public IWorkerManager workerManager
        {
            get
            {
                _workerManager ??= new WorkerManager(NovinDb);
                return _workerManager;
            }
        }

        public IFunctionManager functionManager
        {
            get
            {
                _functionManager ??= new FunctionManager(NovinDb);
                return _functionManager;
            }
        }

        public IAidManager aidManager
        {
            get
            {
                _aidManager ??= new AidManager(NovinDb);
                return _aidManager;
            }
        }

        public async void Dispose()
        {
            await NovinDb.DisposeAsync();
        }

        public async Task SaveChangesAsync()
        {
            await NovinDb.SaveChangesAsync();
        }
    }
}
