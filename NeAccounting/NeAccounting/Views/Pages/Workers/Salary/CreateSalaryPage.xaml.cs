using DomainShared.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Salary.xaml
    /// </summary>
    public partial class CreateSalaryPage : INavigableView<CreateSalaryViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public CreateSalaryViewModel ViewModel { get; }
        public CreateSalaryPage(CreateSalaryViewModel viewModel, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            _snackbarService = snackbarService;
        }

        private async void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {

            ViewModel.WorkerId = ((SuggestBoxViewModel<int>)args.SelectedItem).Id;
            if (!await ViewModel.OnSelect())
            {
                return;
            }
            if (ViewModel.ShiftStatus == DomainShared.Enums.Shift.ByMounth)
            {
                txt_Tax.IsEnabled = true;
                txt_loanInstallment.IsEnabled = true;
                txt_RighOfFood.IsEnabled = true;
                txt_ChildAllowance.IsEnabled = true;
            }
            else
            {
                txt_Tax.Value = 0;
                txt_Tax.IsEnabled = false;

                txt_loanInstallment.Value = 0;
                txt_loanInstallment.IsEnabled = false;

                txt_RighOfFood.Value = 0;
                txt_RighOfFood.IsEnabled = false;

                txt_ChildAllowance.Value = 0;
                txt_ChildAllowance.IsEnabled = false;
            }
            txt_OtherAdditions.IsEnabled = true;
            txt_Othere.IsEnabled = true;
            SetTotalPrice();
            SetTotalPlusPrice();
        }
         
        private void SetTotalPlusPrice()
        {
            var total = (double)(txt_amountOf.Value + txt_overtime.Value + txt_ChildAllowance.Value + txt_OtherAdditions.Value + txt_RighOfFood.Value);
            txt_totalPlus.Text = total.ToString("N");
        }

        private void NumberBox_ValueChanged(object sender, RoutedEventArgs e)
        {
            var input = ((NumberBox)sender).Value;
            if (input != null && input != 0)
            {
                SetTotalPlusPrice();
            }
        }

        private void SetTotalPrice()
        {
            var total = (double)(txt_Aid.Value + txt_Insurance.Value + txt_Tax.Value + txt_loanInstallment.Value + txt_Othere.Value);
            txt_totalPlus.Text = total.ToString("N");
        }

        private void NumberMinesBox_ValueChanged(object sender, RoutedEventArgs e)
        {
            var input = ((NumberBox)sender).Value;
            if (input != null && input != 0)
            {
                SetTotalPlusPrice();
            }
        }
    }
}
