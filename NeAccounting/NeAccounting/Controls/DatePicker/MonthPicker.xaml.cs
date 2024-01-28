using System.Globalization;
using System.Windows.Controls;
namespace NeAccounting.Controls
{
    /// <summary>
    /// Interaction logic for MonthPicker.xaml
    /// </summary>
    public partial class MonthPicker : UserControl
    {
        // ایا تقویم به صورت کامل بارگذازی شده
        private bool IsCalculated = false;

        /// <summary>
        /// تقویم فارسی
        /// </summary>
        private static readonly PersianCalendar persianCalendar = new();


        /// <summary>
        /// load range of Year
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static List<int> LoadYear(int year) => Enumerable.Range(year - 50, 100).ToList();

        //اطلاعات تاریخ امروز 
        private static int currentYear = 1387;
        private static int currentMonth = 10;


        //اطلاعات تاریخ انتخابی 
        private static readonly int selectedYear = 1387;
        private static readonly int selectedMonth = 10;


        /// <summary>
        /// سال انتخابی
        /// </summary>
        public int SelectedYear
        {
            get { return (int)GetValue(SelectedYearProperty); }
            set { SetValue(SelectedYearProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedYear.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedYearProperty =
            DependencyProperty.Register("SelectedYear", typeof(int), typeof(MonthPicker), new PropertyMetadata(currentYear));




        public int SelectedMonth
        {
            get { return (int)GetValue(SelectedMonthProperty); }
            set { SetValue(SelectedMonthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedMonth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedMonthProperty =
            DependencyProperty.Register("SelectedMonth", typeof(int), typeof(MonthPicker), new PropertyMetadata(currentMonth));




        public MonthPicker()
        {
            InitializeComponent();
            DataContext = this;
            IsCalculated = false;
            currentYear = persianCalendar.GetYear(DateTime.Now);
            currentMonth = persianCalendar.GetMonth(DateTime.Now);
            InitialCalculator(currentYear, currentMonth);
        }

        protected virtual void InitialCalculator(int year, int month)
        {

            //select correct month and year
            this.comboBoxMonths.SelectedIndex = month - 1;
            this.comboBoxYear.ItemsSource = LoadYear(year);
            this.comboBoxYear.SelectedItem = year;
            this.lbl_Display.Text = currentYear.ToString() + "/" + currentMonth.ToString();
            IsCalculated = true;
        }

        private void ComboBoxMonths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!IsCalculated)
            {
                return;
            }
            if (sender is not ComboBox cmbox)
            {
                return;
            }
            SelectedMonth = (int)cmbox.SelectedIndex + 1;
            this.lbl_Display.Text = currentYear.ToString() + "/" + currentMonth.ToString();

        }

        private void ComboBoxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (!IsCalculated)
            {
                return;
            }
            if (sender is not ComboBox cmbox)
            {
                return;
            }
            SelectedYear = (int)cmbox.SelectedItem;

            this.lbl_Display.Text = currentYear.ToString() + "/" + currentMonth.ToString();

        }

    }
}
