using DomainShared.Constants;
using DomainShared.Notifications;
using Infrastructure.UnitOfWork;
using Wpf.Ui.Controls;

namespace NeAccounting.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _userName = "";

        [ObservableProperty]
        private string _logInTime = "";

        [ObservableProperty]
        private IEnumerable<NotifViewModel> _notifs;

        public async void OnNavigatedTo()
        {
            if (!_isInitialized)
                await InitializeViewModel();
        }

        public void OnNavigatedFrom()
        {
        }

        private async Task InitializeViewModel()
        {
            UserName = " کاربر  :  " + CurrentUser.CurrentFullName;
            LogInTime = " ورود به برنامه : " + CurrentUser.LogInTime;
            using (BaseUnitOfWork db = new())
            {
                Notifs = await db.NotifRepository.GetNotifs();
            }

            _isInitialized = true;
        }

    }
}
