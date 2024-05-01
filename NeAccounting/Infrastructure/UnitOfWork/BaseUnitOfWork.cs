using Infrastructure.BaseRepositories;
using Infrastructure.EntityFramework;
using NeApplication.IBaseRepositories;

namespace Infrastructure.UnitOfWork
{
    public class BaseUnitOfWork : IDisposable
    {
        readonly BaseDomainDbContext BaseNovin = new();

        private IIdentityUserManager _userManager;
        private INotifManager _notifManager;
        private IBackUpManager _backUpManager;
        private IFinancialYearManager _financialYearManager;

        public IIdentityUserManager UserRepository
        {
            get
            {
                _userManager ??= new UserManager(BaseNovin);
                return _userManager;
            }
        }

        public IBackUpManager BackUpRepository
        {
            get
            {
                _backUpManager ??= new BackUpManager(BaseNovin);
                return _backUpManager;
            }
        }

        public IFinancialYearManager FinancialYearRepository
        {
            get
            {
                _financialYearManager ??= new FinancialYearManager(BaseNovin);
                return _financialYearManager;
            }
        }

        public INotifManager NotifRepository
        {
            get
            {
                _notifManager ??= new NotifManager(BaseNovin);
                return _notifManager;
            }
        }
        public void Dispose() => BaseNovin.Dispose();

        public async Task SaveChangesAsync()
        {
            await BaseNovin.SaveChangesAsync();
        }
    }
}
