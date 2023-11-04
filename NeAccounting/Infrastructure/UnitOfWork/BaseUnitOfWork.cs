using NeApplication.IBaseRepositories;
using Infrastructure.Common;
using Infrastructure.EntityFramework;
using Infrastructure.Common.BaseRepositories.Users;

namespace Infrastructure.UnitOfWork
{
    public class BaseUnitOfWork : IDisposable
    {
        readonly BaseDomainDbContext BaseNovin = new ();

        private IIdentityUserManager _userManager;
        private INotifManager _notifManager;

        public IIdentityUserManager userRepository
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = new UserManager(BaseNovin);
                }
                return _userManager;
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
        public void Dispose()
        {
            BaseNovin.Dispose();
        }
    }
}
