using Application.IBaseRepositories.Users;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Threading.Tasks;

namespace Np_Accounting.ViewModels
{
    class LogInViewModel : ObservableObject
    {
        private readonly IUserManager _userManager;

        public LogInViewModel(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task<Tuple<bool, string>> LogIn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return Tuple.Create(false, "وارد کردن نام کاربری الزامیست!!!");
            }

            if (string.IsNullOrEmpty(password))
            {
                return Tuple.Create(false, "وارد کردن گذرواژه الزامیست");
            }

            if (await _userManager.LogInUser(userName, password))
            {
                return Tuple.Create(true, "ورود با موفقیت انجام شد");
            }

            return Tuple.Create(false, "عدم تطابق نام کاربری و گذرواژه.");
        }
    }
}
