using Infrastructure.UnitOfWork;

namespace NeAccounting.ViewModels
{
    public partial class LogInWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _logInError = "";

        public async Task<bool> LogIn(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                LogInError = "وارد کردن نام کاربری الزامیست !!!";
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                LogInError = "وارد کردن گذرواژه الزامیست !!!";
                return false;
            }
            using (BaseUnitOfWork db = new BaseUnitOfWork())
            {
                if (await db.userRepository.LogInUser(userName, password))
                {
                    LogInError = "ورود با موفقیت انجام شد !!!"; 
                    return true;
                }
            }
            LogInError = "عدم تطابق نام کاربری و گذرواژه !!!";
            return false;
        }
    }
}
