using CommunityToolkit.Mvvm.ComponentModel;
using DomainShared.Constants;
using Wpf.Ui.Common.Interfaces;

namespace Np_Accounting.ViewModels
{
    public partial class DashboardViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string _userName = "";

        public void OnNavigatedTo()
        {
            if (string.IsNullOrEmpty(_userName))
                UserName = CurrentUser.CurrentName;
        }

        public void OnNavigatedFrom()
        {
        }

    }
}
