using Domain.BaseDomain.User;
using DomainShared.ViewModels.Menu;
using DomainShared.ViewModels.Users;
using NeApplication.Common;

namespace NeApplication.IBaseRepositories
{
    public interface IIdentityUserManager : IBaseRepository<IdentityUser>
    {
        /// <summary>
        /// دریافت لیست کاربران سیستم
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<List<UsersListDto>> GetUserList(string name, string mobile);

        /// <summary>
        /// ایجاد کاربر جدید
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="name"></param>
        /// <param name="surName"></param>
        /// <param name="nationalCode"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<(string error, bool isSuccess)> CreateUser(string userName,
                            string name,
                            string surName,
                            string nationalCode,
                            string mobile,
                            List<Guid> permissionIds);

        /// <summary>
        /// دریافت لیست منو های کاربر
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        Task<List<UserMenuDto>> GetUserMenu();

        /// <summary>
        /// بررسی برای ورود کاربر
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<(bool isSuccess, string error)> LogInUser(string userName, string password);

        /// <summary>
        /// تغییر رمز عبور
        /// </summary>
        /// <param name="currentPass"></param>
        /// <param name="NewPass"></param>
        /// <returns></returns>
        Task<(bool isSuccess, string error)> ChangePass(string currentPass, string NewPass);

        /// <summary>
        /// ثبت تم فعلی کاربر بر اساس شناسه کاربر
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        Task UpdateUserTheme(Guid userId, DomainShared.Enums.Themes.Theme theme);

        /// <summary>
        /// بارگذاری تم فعلی کاربر بر اساس شناسه کاربر.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<DomainShared.Enums.Themes.Theme> LoadUserTheme(Guid userId);

        /// <summary>
        /// فعال و غیر فعال کردن کاربر
        /// </summary> 
        /// <param name="Id"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        Task<(string error, bool isSuccess)> SetActiveUser(Guid Id, bool isActive);


    }
}
