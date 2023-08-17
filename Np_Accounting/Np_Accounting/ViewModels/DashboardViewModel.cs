using DomainShared.Constants;
using DomainShared.Notifications;
using Infrastructure.UnitOfWork;

namespace Np_Accounting.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private bool _isInitialized = false;

        [ObservableProperty]
        private string _userName = "";

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
            UserName = CurrentUser.CurrentName;

            using (BaseUnitOfWork db = new BaseUnitOfWork())
            {
                Notifs = await db.NotifRepository.GetNotifs();
            }

            _isInitialized = true;
        }

    }
}
