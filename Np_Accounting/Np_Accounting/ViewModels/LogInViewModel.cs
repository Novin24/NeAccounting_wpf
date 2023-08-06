using CommunityToolkit.Mvvm.ComponentModel;
using Infrastructure.UnitOfWork;
using System;
using System.Threading.Tasks;
using Wpf.Ui.Common.Interfaces;

namespace Np_Accounting.ViewModels
{
    public class LogInViewModel : ObservableObject, INavigationAware
    {
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
            using (BaseUnitOfWork db = new BaseUnitOfWork())
            {

            if (await db.userRepository.LogInUser(userName, password))
            {
                return Tuple.Create(true, "ورود با موفقیت انجام شد");
            }
            }

            return Tuple.Create(false, "عدم تطابق نام کاربری و گذرواژه.");
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
