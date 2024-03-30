using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateCustomerPage.xaml
    /// </summary>
    public partial class UpdateCustomerPage : INavigableView<UpdateCustomerViewModel>
    {
        public UpdateCustomerViewModel ViewModel { get; }

        public UpdateCustomerPage(UpdateCustomerViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_fullName.Focus();
            CalculateTotal();
        }

        private void CashCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
            {
                return;
            }


            if (DataContext is not UpdateCustomerPage cusPage)
            {
                return;
            }

            if (!cusPage.ViewModel.HaveCashCredit)
            {
                txt_CashGrantee.Value = 0;
                txt_CashGrantee.IsEnabled = false;
                CalculateTotal();
                return;
            }
            txt_CashGrantee.IsEnabled = true;

            CalculateTotal();
        }

        private void PromissoryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized) { return; }

            if (DataContext is not UpdateCustomerPage cusPage)
            {
                return;
            }

            if (!cusPage.ViewModel.HavePromissoryNote)
            {
                txt_Grantee.Value = 0;
                txt_Grantee.IsEnabled = false;
                CalculateTotal();
                return;
            }
            txt_Grantee.IsEnabled = true;

            CalculateTotal();
        }

        private void Txt_Grantee_LostFocus(object sender, RoutedEventArgs e)
        {
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            if (DataContext is not UpdateCustomerPage cusPage)
            {
                return;
            }
            var cash = cusPage.ViewModel.CashCredit == null ? 0 : cusPage.ViewModel.CashCredit;
            var promissory = cusPage.ViewModel.PromissoryNote == null ? 0 : cusPage.ViewModel.PromissoryNote;
            var cheque = cusPage.ViewModel.ChequeCredit == null ? 0 : cusPage.ViewModel.ChequeCredit;

            txt_total.Text = String.Format("{0:N0}", cash + promissory + cheque);
        }


        private void Txt_ValueChanged(object sender, RoutedEventArgs e)
        {
            CalculateTotal();
        }
    }
}
