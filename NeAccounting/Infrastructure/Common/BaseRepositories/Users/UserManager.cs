using System.Globalization;
using Common.Utilities;
using Domain.BaseDomain.Menus;
using Domain.BaseDomain.User;
using Domain.NovinEntity.Customers;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Extension;
using DomainShared.ViewModels.Menu;
using DomainShared.ViewModels.Users;
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

        private async Task<IdentityUser> GetUser(string userName)
        {
            for (int i = 0; i < 5; i++)
            {

                try
                {
                    var u = await Entities.FirstOrDefaultAsync(x => x.UserName == userName);
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
            return null;
        }

        public async Task<List<UsersListDto>> GetUserList(string name, string mobile)
        {
            var users = await TableNoTracking
                 .Where(x => string.IsNullOrEmpty(name) || x.Name.Contains(name))
                 .Where(x => string.IsNullOrEmpty(mobile) || x.Mobile.Contains(mobile))
                 .Where(c => c.Id != Guid.Empty)
                 .Select(x => new UsersListDto
                 {
                     Id = x.Id,
                     DisplayName = x.Name + " " + x.SurName,
                     IsActive = x.IsActive,
                     Mobile = x.Mobile,
                     LastSeen = x.LastSeen,
                     UserName = x.UserName,
                 }).OrderBy(t => t.DisplayName).ToListAsync();
            int row = 0;
            var pc = new PersianCalendar();
            foreach (var item in users)
            {
                item.Row = ++row;
                item.ShamsiDate = item.LastSeen.ToShamsiDate(pc);
            }
            return users;
        }

        public async Task<(string error, bool isSuccess)> CreateUser(string userName,
                            string name,
                            string surName,
                            string nationalCode,
                            string mobile,
                            List<Guid> permissionIds)
        {
            if (await TableNoTracking.AnyAsync(t => t.Name == name || t.NationalCode == nationalCode))
                return new(" متسفانه این کاربر در پایگاه داده موجود می‌باشد!!!", false);

            var selectedMenus = await DbContext.Set<Menu>().Where(t => permissionIds.Contains(t.Id)).ToListAsync();

            try
            {

                var t = await Entities.AddAsync(new IdentityUser(userName,
                    name,
                    surName,
                    nationalCode,
                    mobile,
                    SecurityHelper.GetSha512Hash(nationalCode),
                    StringCipher.Encrypt(nationalCode),
                    selectedMenus));
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(07t43493)!!!", false);
            }
            return new(string.Empty, true);
        }


        public async Task<(string error, bool isSuccess)> SetActiveUser(Guid Id, bool isActive)
        {
            try
            {
                var usr
                    = await Entities.FindAsync(Id);

                if (usr == null)
                    return new("مشتری مورد نظر یافت نشد !!!", false);

                if (usr.UserName == "admin")
                    return new( "عملیات ناموفق !!!", false);

                usr.SetActive(isActive);
                Entities.Update(usr);
            }
            catch
            {
                return new(" خطا در اتصال به پایگاه داده code(17t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<List<UserMenuDto>> GetUserMenu()
        {
            var menus = await TableNoTracking
                .Where(t => t.Id == CurrentUser.CurrentUserId)
                .Select(t => t.Menus)
                .FirstOrDefaultAsync();

            if (menus == null) return [];

            var result = new List<UserMenuDto>();

            var rootItems = menus.Where(t => t.Root == 0)
                    .OrderBy(t=> t.Level)
                    .ToList();

            foreach (var item in rootItems)
            {
                var parentDto = new UserMenuDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsParent = true
                };

                var children = menus
                    .Where(c => c.ParentId == item.Id)
                    .OrderBy(t=> t.Level)
                    .Select(n => new UserMenuDto
                    {
                        Id = n.Id,
                        Name = n.Name,
                        IsParent = false,
                        Parent = parentDto
                    })
                    .ToList();

                parentDto.Children = children;

                result.Add(parentDto);
            }

            return result;

            //return [.. menus
            //    .Where(t => t.Root == 0)
            //    .Select(t => new UserMenuDto
            //    {
            //        Id = t.Id,
            //        Name = t.DisplayName,
            //        IsParent = true,
            //        Children = [.. menus.Where(c => c.ParentId == t.Id).Select(n => new UserMenuDto
            //        {
            //            Name = n.DisplayName,
            //            Id = n.Id,
            //            IsParent = false
            //        })]
            //    })];
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

            if (!user.IsActive) { return (false, "متسفانه دسترسی شما توسط ادمین \n محدود گردیده ! ! !"); }

            if (user.UserName == null) { return (false, "خطا در اتصال به پایگاه داده ! ! !"); }

            if (user.PasswordHash != passHash) { return (false, "عدم تطابق نام کاربری و گذرواژه !!!"); }

            CurrentUser.CurrentFullName = user.Name + " " + user.SurName;
            CurrentUser.CurrentName = user.Name;
            CurrentUser.CurrentUserName = user.UserName;
            CurrentUser.CurrentUserId = user.Id;
            CurrentUser.LogInTime = DateTime.Now.ToString("HH:mm:ss");
            await UpdateUserSeen(user);
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
                user.SetPass(SecurityHelper.GetSha512Hash(newPass));
                user.SetTemp(StringCipher.Encrypt(newPass));
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


        private async Task UpdateUserSeen(IdentityUser user)
        {
            user.UpdateLastSeen();
            Entities.Update(user);
            await DbContext.SaveChangesAsync();
        }
    }
}
