using Domain.BaseDomain.User;
using NeApplication.Common;

namespace NeApplication.IBaseRepositories
{
    public interface IIdentityUserManager : IBaseRepository<IdentityUser>
    {
        Task<bool> LogInUser(string userName, string password);
        Task<IdentityUser> GetUser(string userName);
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


	}
}
