using NeAccounting.Helpers.Extention;
using NeAccounting.ViewModels;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// صفحه ایجاد کارگران
    /// </summary>
    public partial class CreateWorkerPage : INavigableView<CreateWorkerViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public CreateWorkerViewModel ViewModel { get; }
        public CreateWorkerPage(CreateWorkerViewModel viewModel, ISnackbarService snackbarService)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_name.Focus();
            _snackbarService = snackbarService;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
            {
                return;
            }
            ViewModel.Shift = DomainShared.Enums.Shift.ByHour;

            txt_dayInMonth.IsEnabled = false;
            txt_dayInMonth.Value = 0;
            txt_MonthPrice.IsEnabled = false;
            txt_MonthPrice.Value = 0;
            txt_insurancePerimum.IsEnabled = false;
            txt_insurancePerimum.Value = 0;
            txt_overtimePriceMonth.IsEnabled = false;
            txt_overtimePriceMonth.Value = 0;

            txt_ShiftPrice.IsEnabled = true;
            txt_overTimeShift.IsEnabled = true;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
            {
                return;
            }
            ViewModel.Shift = DomainShared.Enums.Shift.ByMounth;

            txt_dayInMonth.IsEnabled = true;
            txt_MonthPrice.IsEnabled = true;
            txt_insurancePerimum.IsEnabled = true;
            txt_overtimePriceMonth.IsEnabled = true;

            txt_ShiftPrice.IsEnabled = false;
            txt_ShiftPrice.Value = 0;
            txt_overTimeShift.IsEnabled = false;
            txt_overTimeShift.Value = 0;
        }

        [RelayCommand]
        private async Task OnCreate()
        {

            Btn_submit.Focus();
            await ViewModel.CreateCommand.ExecuteAsync(null);
        }
    }
}
