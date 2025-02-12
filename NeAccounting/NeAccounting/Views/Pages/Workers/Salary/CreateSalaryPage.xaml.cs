using DomainShared.ViewModels;
using NeAccounting.Controls;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Salary.xaml
    /// </summary>
    public partial class CreateSalaryPage : INavigableView<CreateSalaryViewModel>
    {

        public CreateSalaryViewModel ViewModel { get; }
        private long Additions = 0;
        private long Deductions = 0;
        private long LeftOver = 0;
        public CreateSalaryPage(CreateSalaryViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private async void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
            {
                return;
            }
            var id = ((PersonnerlSuggestBoxViewModel)args.SelectedItem).Id;
            ViewModel.WorkerId = id;
            await ReloadSalary();
        }

        private void SetTotalPlusPrice()
        {
            var total = txt_amountOf.Value + txt_overtime.Value + txt_ChildAllowance.Value + txt_OtherAdditions.Value + txt_RighOfFood.Value;
            Additions = total;
            LeftOver = Additions - Deductions;
            ViewModel.LeftOver = LeftOver;
            txt_totalPlus.Text = total.ToString("N0");
            lbl_leftOver.Text = Math.Abs(LeftOver).ToString("N0");
            if (LeftOver != 0)
            {
                if (LeftOver > 0)
                {
                    lbl_status.Text = "طلبکار";
                    lbl_status.Foreground = new SolidColorBrush(Colors.IndianRed);
                }
                else
                {
                    lbl_status.Text = "بدهکار";
                    lbl_status.Foreground = new SolidColorBrush(Colors.DarkSeaGreen);
                }

            }
        }

        private void NumberBox_ValueChanged(object sender, RoutedEventArgs e)
        {
            var input = ((MoneyPack)sender).Value;
            if (input != 0)
            {
                SetTotalPlusPrice();
            }
        }

        private void SetTotalPrice()
        {
            var total = txt_Aid.Value + txt_Insurance.Value + txt_Tax.Value + txt_loanInstallment.Value + txt_Othere.Value;
            Deductions = total;
            LeftOver = Additions - Deductions;
            ViewModel.LeftOver = LeftOver;
            txt_totalMines.Text = total.ToString("N0");
            lbl_leftOver.Text = Math.Abs(LeftOver).ToString("N0");
            if (LeftOver != 0)
            {
                if (LeftOver > 0)
                {
                    lbl_status.Text = "طلبکار";
                    lbl_status.Foreground = new SolidColorBrush(Colors.IndianRed);
                }
                else
                {
                    lbl_status.Text = "بدهکار";
                    lbl_status.Foreground = new SolidColorBrush(Colors.DarkSeaGreen);
                }

            }

        }

        private void NumberMinesBox_ValueChanged(object sender, RoutedEventArgs e)
        {
            var input = ((MoneyPack)sender).Value;
            if (input != 0)
            {
                SetTotalPrice();
            }
        }

        private async void dtp_MonthChosen(object sender, RoutedPropertyChangedEventArgs<byte?> e)
        {
            if (!IsInitialized)
            {
                return;
            }
            if (ViewModel.WorkerId == null)
                return;

            await ReloadSalary();
        }

        private async void dtp_YearChosen(object sender, RoutedPropertyChangedEventArgs<int?> e)
        {
            if (!IsInitialized)
            {
                return;
            }
            if (ViewModel.WorkerId == null)
                return;

            await ReloadSalary();
        }

        private async Task ReloadSalary()
        {
            if (!await ViewModel.OnSelect())
            {
                txt_Aid.Value = txt_Insurance.Value = txt_Tax.Value = txt_loanInstallment.Value = txt_Othere.Value = txt_amountOf.Value =
                txt_overtime.Value = txt_ChildAllowance.Value = txt_OtherAdditions.Value = txt_RighOfFood.Value = 0;
                lbl_leftOver.Text = txt_totalPlus.Text = txt_totalMines.Text = "0";
                lbl_status.Text = "تسویه";
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
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_name.Focus();
        }
        [RelayCommand]
        private async Task OnCreate()
        {
            Btn_submit.Focus();
            await ViewModel.CreateCommand.ExecuteAsync(null);
        }

        private void txt_CustomerName_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = ViewModel.Loding;
        }
    }
}
