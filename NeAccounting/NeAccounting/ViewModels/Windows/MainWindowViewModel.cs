using System.Collections.ObjectModel;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using NeAccounting.Pages;
using Infrastructure.UnitOfWork;

namespace NeAccounting.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "Novin Acconting";

        [ObservableProperty]
        private string _logInError = "";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home32 },
                TargetPageType = typeof(DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "دریافتی",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Payment32 },
                TargetPageType = typeof(RecPage)
            },
            new NavigationViewItem()
            {
                Content = "پرداختی",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Payment32 },
                TargetPageType = typeof(PayPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings20 },
                TargetPageType = typeof(SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };

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
