using System.Globalization;
using System.Windows.Controls;
namespace NeAccounting.Controls
{
    /// <summary>
    /// Interaction logic for MonthPicker.xaml
    /// </summary>
    public partial class MonthPicker : UserControl
    {
        #region Propertis
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
        #endregion

        #region LableName
        public string LabelName
        {
            get { return (string)GetValue(LableNameProperty); }
            set { SetValue(LableNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LableName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LableNameProperty =
            DependencyProperty.Register("LableName", typeof(string), typeof(MonthPicker), new PropertyMetadata(string.Empty, SetLabelName));

        private static void SetLabelName(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not MonthPicker npack)
                return;

            if (e.NewValue == e.OldValue)
                return;

            npack.lbl_name.Text = e.NewValue.ToString();
        }
        #endregion

        #region SelectedDate
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
        #endregion

        #region ctor
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
            comboBoxMonths.SelectedIndex = month - 1;
            comboBoxYear.ItemsSource = LoadYear(year);
            comboBoxYear.SelectedItem = year;
            lbl_Display.Text = currentYear.ToString() + "/" + currentMonth.ToString();
            IsCalculated = true;
        }
        #endregion

        #region event
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
            lbl_Display.Text = SelectedYear.ToString() + "/" + SelectedMonth.ToString();

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
            lbl_Display.Text = SelectedYear.ToString() + "/" + SelectedMonth.ToString();

        }
        #endregion
    }
}
