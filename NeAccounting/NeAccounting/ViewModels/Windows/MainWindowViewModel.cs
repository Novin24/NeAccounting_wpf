using System.Collections.ObjectModel;
using NeAccounting.Helpers;
using NeAccounting.Views.Pages;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        public MainWindowViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [ObservableProperty]
        private string _applicationTitle = "Novin Acconting";

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "داشبورد",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home32 },
                TargetPageType = typeof(DashboardPage)
            },

            new NavigationViewItemSeparator(),

        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Name="Settings",
                Content = "تنظیمات کاربری",
                Icon = new SymbolIcon { Symbol = SymbolRegular.PersonSettings20 ,Filled =true },
                MenuItems = new ObservableCollection<NavigationViewItem>
                {
					#region systemUser
					new() {Name="Users",Content = "کاربران سیستم",TargetPageType = typeof(UserListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.PersonSettings20,Filled = true} },
					#endregion
                    #region ChangePassword
                    new() {Name="ChangePassword",Content = "تغییر رمز عبور",TargetPageType = typeof(ChangePassword) , Icon = new SymbolIcon{ Symbol = SymbolRegular.LockMultiple20,Filled = true} },
                #endregion
                    #region Notification
                    new() {Name="Notes",Content = "یادآورها",TargetPageType = typeof(NotificationListPage) , Icon = new SymbolIcon{ Symbol = SymbolRegular.CalendarCheckmark20 , Filled = true} },
                    new() {Content = "ایجاد یادآور",Icon = new SymbolIcon { Symbol = SymbolRegular.ReadingListAdd28},TargetPageType = typeof(CreateNotificationPage),Visibility = Visibility.Collapsed},
                #endregion

                }
            },
            new NavigationViewItem()
            {
                Content = "تنظیمات برنامه",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings20 , Filled =true },
                TargetPageType = typeof(SettingsPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };

        [RelayCommand]
        private void OnAddClick(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                return;
            }

            Type? pageType = NameToPageTypeConverter.Convert(parameter);

            if (pageType == null)
            {
                return;
            }

            _ = _navigationService.Navigate(pageType);
        }

    }
}
