using NeApplication.Common;
using Domain.BaseDomain.User;

namespace NeApplication.IBaseRepositories
{
    public interface IUserManager : IBaseRepository<IdentityUser>
    {
        Task<bool> LogInUser(string userName, string password);
        Task<IdentityUser> GetUser(string userName);
    }
}
