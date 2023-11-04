using Common.Utilities;
using Domain.BaseDomain.User;
using Domain.NovinEntity.Customers;
using DomainShared.Constants;
using DomainShared.ViewModels;
using Infrastructure.Common.Repositories;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;
using NeApplication.IRepositoryies;

namespace Infrastructure.Common.BaseRepositories.Users
{
    public class UserManager : BaseRepository<IdentityUser>, IIdentityUserManager
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

    public class CustomerManager : Repository<Customer>, ICustomerManager
    {
        public CustomerManager(NovinDbContext context) : base(context) { }


        public Task<List<SuggestBoxViewModel<Guid>>> GetDisplayUser()
        {
            return TableNoTracking.Select(x => new SuggestBoxViewModel<Guid>
            {
                Id = x.Id,
                DisplayName = x.Name

            }).ToListAsync();
        }

    }


}
