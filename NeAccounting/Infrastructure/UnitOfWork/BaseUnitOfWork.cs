using Infrastructure.Common.BaseRepositories.Users;
using Infrastructure.EntityFramework;
using Infrastructure.Repositories;
using NeApplication.IBaseRepositories;

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
                _userManager ??= new UserManager(BaseNovin);
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
        public void Dispose() => BaseNovin.Dispose();
    }
}
