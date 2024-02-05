using System.ComponentModel;
using System.Globalization;
using System.Windows.Controls;
namespace NeAccounting.Controls
{
    /// <summary>
    /// Interaction logic for MonthPicker.xaml
    /// </summary>
    [DefaultEvent("SelectedDateChanged")]
    [DefaultProperty("SelectedMon")]
    public partial class MonthPicker : UserControl
    {
        #region Propertis
        // ایا تقویم به صورت کامل بارگذازی شده
        private static bool IsCalculated = false;

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
        private readonly int currentYear = 1387;
        private readonly byte currentMonth = 10;

        //اطلاعات تاریخ انتخابی 
        private static int? selectedYea;
        private static byte? selectedMonth;
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
        /// سال انتخاب شده
        /// </summary>
        public int? SelectedYear
        {
            get { return (int?)GetValue(SelectedYearProperty); }
            set { SetValue(SelectedYearProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedYear.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedYearProperty =
            DependencyProperty.Register("SelectedYear", typeof(int?), typeof(MonthPicker), new PropertyMetadata(null, SelectYear));

        private static void SelectYear(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is not MonthPicker mp)
                return;

            if (args.NewValue == args.OldValue)
                return;
            IsCalculated = false;
            selectedYea = (int?)args.NewValue;
            mp.InitialYear(selectedYea);
            IsCalculated = true;
        }




        /// <summary>
        /// ماه انتخاب شده
        /// </summary>
        public byte? SelectedMon
        {
            get { return (byte?)GetValue(SelectedMonProperty); }
            set { SetValue(SelectedMonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedMonth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedMonProperty =
            DependencyProperty.Register("SelectedMon", typeof(byte?), typeof(MonthPicker),
                new PropertyMetadata(null, new PropertyChangedCallback(OnMonthChanged)));

        private static void OnMonthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (sender is not MonthPicker c)
                return;

            if (args.NewValue == args.OldValue)
                return;

            IsCalculated = false;
            selectedMonth = (byte?)args.NewValue;
            c.InitialMonth(selectedMonth);
            RoutedPropertyChangedEventArgs<byte?> e = new((byte?)args.OldValue, (byte?)args.NewValue, DateChosenEvent);
            c.OnDateChanged(e);
            IsCalculated = true;
        }
        #endregion

        #region ctor
        public MonthPicker()
        {
            IsCalculated = false;
            InitializeComponent();
            this.currentYear = persianCalendar.GetYear(DateTime.Now);
            this.currentMonth = Convert.ToByte(persianCalendar.GetMonth(DateTime.Now));
            selectedMonth = SelectedMon;
            selectedYea = SelectedYear;
            InitialMonth(selectedMonth);
            InitialYear(selectedYea);
            IsCalculated = true;
        }

        protected virtual void InitialMonth(byte? month)
        {
            comboBoxMonths.SelectedIndex = month - 1 ?? currentMonth - 1;
            if (SelectedMon != null && SelectedYear != null)
                lbl_Display.Text = SelectedYear.ToString() + " / " + SelectedMon.ToString();
        }

        protected virtual void InitialYear(int? year)
        {
            comboBoxYear.ItemsSource = LoadYear(year ?? currentYear);
            comboBoxYear.SelectedItem = year ?? currentYear;
            if (SelectedMon != null && SelectedYear != null)
                lbl_Display.Text = SelectedYear.ToString() + " / " + SelectedMon.ToString();
        }

        #endregion

        #region CustomeEvent
        /// <summary>
        /// Event occurs when the user selects an item from the recommended ones.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<byte?> DateChosen
        {
            add => AddHandler(DateChosenEvent, value);
            remove => RemoveHandler(DateChosenEvent, value);
        }

        /// <summary>
        /// Routed event for <see cref="DateChosen"/>.
        /// </summary>
        public static readonly RoutedEvent DateChosenEvent = EventManager.RegisterRoutedEvent(
            nameof(DateChosen),
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<byte?>),
            typeof(MonthPicker)
        );


        protected virtual void OnDateChanged(RoutedPropertyChangedEventArgs<byte?> args)
        {
            RaiseEvent(args);
        }
        #endregion

        #region event
        private void ComboBoxMonths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsCalculated || sender is not ComboBox cmbox)
            {
                return;
            }
            if (SelectedYear == null)
            {
                IsCalculated = false;
                SelectedYear = currentYear;
                selectedYea = currentYear;
            }
            IsCalculated = false;
            selectedMonth = Convert.ToByte(cmbox.SelectedIndex + 1);
            SelectedMon = selectedMonth;
        }

        private void ComboBoxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsCalculated || sender is not ComboBox cmbox)
            {
                return;
            }
            IsCalculated = false;
            selectedYea = (int)cmbox.SelectedItem;
            SelectedYear = (int)cmbox.SelectedItem;
            if (SelectedMon == null)
            {
                IsCalculated = false;
                SelectedMon = currentMonth;
                selectedMonth = currentMonth;
            }
            else
            {
                RoutedPropertyChangedEventArgs<byte?> re = new(selectedMonth, selectedMonth, DateChosenEvent);
                RaiseEvent(re);
            }
        }

        private void Dismiss_Click(object sender, RoutedEventArgs e)
        {
            lbl_Display.Text = string.Empty;
            IsCalculated = false;
            SelectedYear = null;
            IsCalculated = false;
            SelectedMon = null;
        }
        #endregion
    }
}
