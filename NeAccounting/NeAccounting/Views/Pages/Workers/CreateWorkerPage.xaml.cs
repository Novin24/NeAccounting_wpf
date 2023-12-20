using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// صفحه ایجاد کارگران
    /// </summary>
    public partial class CreateWorkerPage : INavigableView<CreateWorkerViewModel>
    {
        public CreateWorkerViewModel ViewModel { get; }
        public DateTime dateNow { get; set; } = DateTime.Now.AddYears(1);
        public CreateWorkerPage(CreateWorkerViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_name.Focus();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!IsInitialized)
            {
                return;
            }
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
            txt_dayInMonth.IsEnabled = true;
            txt_MonthPrice.IsEnabled = true;
            txt_insurancePerimum.IsEnabled = true;
            txt_overtimePriceMonth.IsEnabled = true;

            txt_ShiftPrice.IsEnabled = false;
            txt_ShiftPrice.Value = 0;
            txt_overTimeShift.IsEnabled = false;
            txt_overTimeShift.Value = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ts = dtp.SelectedDate;
            var sdfd = dtp.DisplayDate;
            dtp.SelectedDate = DateTime.Now.AddYears(2);
            var t = dtp.persianCalendar.SelectedDate = DateTime.Now .AddYears(2);
        }
    }
}
