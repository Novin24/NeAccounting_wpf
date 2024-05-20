using NeAccounting.ViewModels;

namespace NeAccounting.Windows
{
    /// <summary>
    /// Interaction logic for CreateCustomerWindow.xaml
    /// </summary>
    public partial class CreateCustomerWindow
    {
        public HotCreateCustomerViewModel ViewModel { get; }

        public CreateCustomerWindow(HotCreateCustomerViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        [RelayCommand]
        private void OnClose()
        {
            this.Visibility = Visibility.Hidden;
        }

        [RelayCommand]
        private async Task OnCreateCustomer()
        {
            if (DataContext is not CreateCustomerWindow ccw)
            {
                return;
            }

            await ccw.ViewModel.CreateCustomerCommand.ExecuteAsync(this);
        }

    }
}
