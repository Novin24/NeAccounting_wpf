using Application.Common;
using Domain.BaseDomain.User;

namespace Application.IBaseRepositories
{
    public interface IUserManager : IBaseRepository<IdentityUser>
    {
        Task<bool> LogInUser(string userName, string password);
        Task<IdentityUser> GetUser(string userName);
    }
}
