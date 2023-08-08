using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.UnitOfWork;
using System;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;

namespace Np_Accounting.ViewModels
{
    public partial class LogInViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string _logInError = "txt";

        public async Task<bool> LogIn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                LogInError = "وارد کردن نام کاربری الزامیست!!!";
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                LogInError = "وارد کردن گذرواژه الزامیست";
                return false;
            }
            using (BaseUnitOfWork db = new BaseUnitOfWork())
            {
                if (await db.userRepository.LogInUser(userName, password))
                {
                    LogInError = "ورود با موفقیت انجام شد"; 
                    return true;
                }
            }
            LogInError = "عدم تطابق نام کاربری و گذرواژه.";
            return false;
        }

        public void OnNavigatedFrom()
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo()
        {
            throw new NotImplementedException();
        }
    }
}
