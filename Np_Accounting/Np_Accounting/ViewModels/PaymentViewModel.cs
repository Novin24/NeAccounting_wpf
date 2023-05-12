using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wpf.Ui.Common.Interfaces;

namespace Np_Accounting.ViewModels
{
    public partial class PaymentViewModel : ObservableObject, INavigationAware
    {
        private int _payCount = 100;

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        [RelayCommand]
        private void OnPay()
        {
            PayCount++;
        }
    }
}
