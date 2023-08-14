using Application.IBaseRepositories;
using Common.Utilities;
using Domain.BaseDomain.User;
using DomainShared.Constants;
using Infrastructure.Common.BaseRepositories;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common
{
    public class UserManager : BaseRepository<IdentityUser>, IUserManager
    {
        public UserManager(BaseDomainDbContext context) : base(context) { }

        public Task<IdentityUser> GetUser(string userName)
        {
            return TableNoTracking.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<bool> LogInUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            var passHash = SecurityHelper.GetSha512Hash(password);

            var user = await GetUser(userName);

            if (user == null) { return false; }

            if (user.PasswordHash != passHash) { return false; }

            CurrentUser.CurrentFullName = user.Name + " " + user.SurName;
            CurrentUser.CurrentName = user.Name;
            CurrentUser.CurrentUserName = user.UserName;
            CurrentUser.CurrentUserId = user.Id;
            return true;
        }
    }
}
