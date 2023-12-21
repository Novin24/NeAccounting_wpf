using DomainShared.Enums;
using NeAccounting.Helpers.Extention;
using NeAccounting.ViewModels;
using System.Text.RegularExpressions;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// صفحه به روز رسانی کارگران
    /// </summary>
    public partial class UpdateWorkerPage : INavigableView<UpdateWorkerViewModel>
    {
        private readonly ISnackbarService _snackbarService;
        public UpdateWorkerViewModel ViewModel { get; }
        public UpdateWorkerPage(UpdateWorkerViewModel viewModel, ISnackbarService snackbarService)
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
            ViewModel.WorkerShift = Shift.ByHour;

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
            ViewModel.WorkerShift = Shift.ByMounth;

            txt_dayInMonth.IsEnabled = true;
            txt_MonthPrice.IsEnabled = true;
            txt_insurancePerimum.IsEnabled = true;
            txt_overtimePriceMonth.IsEnabled = true;

            txt_ShiftPrice.IsEnabled = false;
            txt_ShiftPrice.Value = 0;
            txt_overTimeShift.IsEnabled = false;
            txt_overTimeShift.Value = 0;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox txt)
            {
                Regex regex = NumberOnly();
                txt.Text = txt.Text.Trim();
                if (!regex.IsMatch(txt.Text))
                {
                    txt.Text = string.Empty;
                    _snackbarService.Show("خطا", "شماره موبایل وارد شده نامعتبر میباشد !!!", ControlAppearance.Caution, new SymbolIcon(SymbolRegular.Warning20), TimeSpan.FromMilliseconds(2000));
                }
            }

        }

        private void Txt_NationalCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox txt)
            {
                if (!txt.Text.ValidNationalCode(_snackbarService))
                {
                    txt.Text = string.Empty;
                }
            }
        }

        [GeneratedRegex(@"^(?:0|98|\+98|\+980|0098|098|00980)?(9\d{9})$")]
        private static partial Regex NumberOnly();
    }
}
