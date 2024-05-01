using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateCustomerPage.xaml
    /// </summary>
    public partial class CreateCustomerPage : INavigableView<CreateCustomerViewModel>
    {
        public CreateCustomerViewModel ViewModel { get; }

        public CreateCustomerPage(CreateCustomerViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_fullName.Focus();
        }

        private void CashCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cmb)
            {
                if (!cmb.IsChecked.Value)
                {
                    txt_CashGrantee.Value = 0;
                    ViewModel.CashCredit = 0;
                    txt_CashGrantee.IsEnabled = false;
                    CalculateTotal();
                    return;
                }
                txt_CashGrantee.IsEnabled = true;
            }
            CalculateTotal();
        }

        private void PromissoryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cmb)
            {
                if (!cmb.IsChecked.Value)
                {
                    txt_Grantee.Value = 0;
                    ViewModel.PromissoryNote = 0;
                    txt_Grantee.IsEnabled = false;
                    CalculateTotal();
                    return;
                }
                txt_Grantee.IsEnabled = true;
            }
            CalculateTotal();
        }

        private void Txt_Grantee_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            var cash = txt_CashGrantee.Value == null ? 0 : txt_CashGrantee.Value;
            var promissory = txt_Grantee.Value == null ? 0 : txt_Grantee.Value;

            txt_total.Text = String.Format("{0:N0}", cash + promissory);
        }

        private void Txt_ValueChanged(object sender, RoutedEventArgs e)
        {
            CalculateTotal();
        }

        [RelayCommand]
        private async Task OnCreateCustomer()
        {
            Btn_submit.Focus();
            await ViewModel.CreateCustomerCommand.ExecuteAsync(null);
        }
    }
}
