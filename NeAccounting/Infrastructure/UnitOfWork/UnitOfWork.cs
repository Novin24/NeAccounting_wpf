using Infrastructure.EntityFramework;
using Infrastructure.Repositories;
using NeApplication.IRepositoryies;
using NeApplication.IRepositoryies.Expens;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {
        readonly NovinDbContext NovinDb = new();

        private ICustomerManager _customerManager;

        private IMaterialManager _materialManager;

        private IWorkerManager _workerManager;

        private IUnitManager _unitManager;

        private IDocumentManager _documentManager;

        private IExpenseManager _expenseManager;

        public ICustomerManager CustomerManager
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

        public IMaterialManager MaterialManager
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
     

        public IUnitManager UnitManager
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

        public IWorkerManager WorkerManager
        {
            get
            {
                _workerManager ??= new WorkerManager(NovinDb);
                return _workerManager;
            }
        }

        public IDocumentManager DocumentManager
        {
            get
            {
                _documentManager ??= new DocumentManager(NovinDb);
                return _documentManager;
            }
        }

        public IExpenseManager ExpenseManager
        {
            get
            {
                _expenseManager ??= new ExpenseManager(NovinDb);
                return _expenseManager;
            }
        }

        public async void Dispose() => await NovinDb.DisposeAsync();

        public async Task SaveChangesAsync()
        {
            await NovinDb.SaveChangesAsync();
        }
    }
}
