using Common.Utilities;
using Domain.BaseDomain.User;
using DomainShared.Constants;
using Infrastructure.EntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;
using Serilog;

namespace Infrastructure.BaseRepositories
{
    public class UserManager : BaseRepository<IdentityUser>, IIdentityUserManager
    {
        public UserManager(BaseDomainDbContext context) : base(context) { }

        private async Task<IdentityUser?> GetUser(string userName)
        {
            for(int i = 0; i < 5; i++)
            {

                try
                {
                    var u = await TableNoTracking.FirstOrDefaultAsync(x => x.UserName == userName);
                    return u;
                }
                catch (SqlException ex) 
                {
                    Log.Error(ex, "LogIn Error, code: (47ls3513)");
                    await Task.Delay(5000); // Wait before retrying
                    continue;
                }
                catch (InvalidOperationException ex) 
                {
                    Log.Error(ex, "LogIn Error, code: (47hs4923)");
                    await Task.Delay(5000); // Wait before retrying
                    continue;
                }
                catch (Exception ex) 
                {
                    Log.Error(ex, "LogIn Error, code: (46hs7223)");
                    await Task.Delay(5000); // Wait before retrying
                    continue;
                }
            }
            return new IdentityUser();
        }

        private async Task<IdentityUser?> GetUser(Guid id)
        {
            return await TableNoTracking.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(bool isSuccess, string error)> LogInUser(string userName, string password)
        {
            var passHash = SecurityHelper.GetSha512Hash(password);

            var user = await GetUser(userName);

            if (user == null) { return (false, "عدم تطابق نام کاربری و گذرواژه !!!"); }

            if (user.UserName == null) { return (false, "خطا در اتصال به پایگاه داده ! ! !"); }

            if (user.PasswordHash != passHash) { return (false, "عدم تطابق نام کاربری و گذرواژه !!!"); }

            CurrentUser.CurrentFullName = user.Name + " " + user.SurName;
            CurrentUser.CurrentName = user.Name;
            CurrentUser.CurrentUserName = user.UserName;
            CurrentUser.CurrentUserId = user.Id;
            CurrentUser.LogInTime = DateTime.Now.ToString("HH:mm:ss");
            return (true, "");
        }


        public async Task<(bool isSuccess, string error)> ChangePass(string currentPass, string newPass)
        {
            var passHash = SecurityHelper.GetSha512Hash(currentPass);

            var user = await Entities.FirstOrDefaultAsync(t => t.Id == CurrentUser.CurrentUserId);

            if (user == null) { return new(false, "کاربر یافت نشد!!"); }

            if (user.PasswordHash != passHash) { return new(false, "رمز عبور فعلی نامعتبر!!!"); }

            try
            {
                var newPassHash = SecurityHelper.GetSha512Hash(newPass);
                user.PasswordHash = newPassHash;
                user.Temp = StringCipher.Encrypt(newPass);
                Entities.Update(user);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
            return (true, string.Empty);
        }
        public async Task UpdateUserTheme(Guid userId, DomainShared.Enums.Themes.Theme theme)
        {
            var user = await GetUser(userId);
            if (user != null)
            {
                user.CurrentTheme = theme; // فرض بر این است که این فیلد در IdentityUser وجود دارد
                Entities.Update(user);
            }
        }
        public async Task<DomainShared.Enums.Themes.Theme> LoadUserTheme(Guid userId)
        {
            var user = await GetUser(userId);
            if (user != null)
            {
                return user.CurrentTheme; // فرض بر این است که این فیلد در IdentityUser وجود دارد
            }
            return DomainShared.Enums.Themes.Theme.Dark; // مقدار پیش‌فرض در صورت عدم وجود کاربر
        }

    }


}
