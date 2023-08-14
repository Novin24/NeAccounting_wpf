using Application.IBaseRepositories;
using Infrastructure.Common;
using Infrastructure.EntityFramework;

namespace Infrastructure.UnitOfWork
{
    public class BaseUnitOfWork : IDisposable
    {
        readonly BaseDomainDbContext BaseNovin = new ();

        private IUserManager _userManager;
        private INotifManager _notifManager;

        public IUserManager userRepository
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
