using Application.IBaseRepositories.Users;
using Infrastructure.Common.Repositories.Users;
using Infrastructure.EntityFramework;

namespace Infrastructure.UnitOfWork
{
    public class BaseUnitOfWork : IDisposable
    {
        readonly BaseDomainDbContext BaseNovin = new ();

        private IUserManager _userManager;
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
        public void Dispose()
        {
            BaseNovin.Dispose();
        }
    }
}
