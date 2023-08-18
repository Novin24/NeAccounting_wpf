namespace Np_Accounting.ViewModels
{
    public partial class PaymentViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _payCount = 125;

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
