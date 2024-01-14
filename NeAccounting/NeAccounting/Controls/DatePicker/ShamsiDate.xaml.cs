using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;
namespace NeAcconting.Controls.DatePicker
{
    /// <summary>
    /// Interaction logic for ShamsiDatePicker.xaml
    /// </summary>
    public partial class ShamsiDate : UserControl
    {

        #region fields
        // ایا تقویم به صورت کامل بارگذازی شده
        private bool IsCalculated = false;
        //\\

        private static PersianCalendar persianCalendar = new PersianCalendar();
        public event RoutedEventHandler Click;
        public event RoutedEventHandler MKeyDown;

        //اطلاعات تاریخ امروز 
        private readonly int currentYear = 1387;
        private readonly int currentMonth = 10;
        private readonly int currentDay = 1;


        //اطلاعات تاریخ انتخابی 
        private static int selectedYear = 1387;
        private static int selectedMonth = 10;
        private static int selectedDay = 1;
        private static int selectedBtnIndex = 0;

        //برای حرکت بین ماه ها
        //به شمسی
        private int yearForNavigating = 1387;
        private int monthForNavigating = 10;

        private readonly Dictionary<int, DateTime> cal = [];

        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set
            { SetValue(SelectedDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(ShamsiDate), new PropertyMetadata(DateTime.Now, SelectedDatePropertyChenged));


        private static void SelectedDatePropertyChenged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is not ShamsiDate shamsiDate)
                return;

            if (((DateTime)args.NewValue).Date == ((DateTime)args.OldValue).Date)
                return;
            selectedYear = persianCalendar.GetYear((DateTime)args.NewValue);
            selectedMonth = persianCalendar.GetMonth((DateTime)args.NewValue);
            selectedDay = persianCalendar.GetDayOfMonth((DateTime)args.NewValue);
            shamsiDate.InitialCalculator(selectedYear, selectedMonth, selectedDay);
        }



        public string PersianSelectedDate
        {
            get { return (string)GetValue(PersianSelectedDateProperty); }
            set { SetValue(PersianSelectedDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PersianSelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PersianSelectedDateProperty =
            DependencyProperty.Register("PersianSelectedDate", typeof(string), typeof(ShamsiDate), new PropertyMetadata(string.Empty));

        #endregion

        public ShamsiDate()
        {
            InitializeComponent();
            // Insert code required on object creation below this point
            this.currentYear = persianCalendar.GetYear(DateTime.Now);
            this.currentMonth = persianCalendar.GetMonth(DateTime.Now);
            this.currentDay = persianCalendar.GetDayOfMonth(DateTime.Now);
            selectedYear = persianCalendar.GetYear(SelectedDate);
            selectedMonth = persianCalendar.GetMonth(SelectedDate);
            selectedDay = persianCalendar.GetDayOfMonth(SelectedDate);
            InitialCalculator(currentYear, currentMonth, currentDay);
        }


        protected virtual void InitialCalculator(int year, int month, int day)
        {
            IsCalculated = false;
            LoadXMLFile();
            DataContext = this;
            //select correct month and year
            this.comboBoxMonths.SelectedIndex = month - 1;
            this.comboBoxYear.ItemsSource = LoadYear(year);
            this.comboBoxYear.SelectedItem = year;

            //Fill the selected date
            PersianSelectedDate = string.Concat(year, "/", month, "/", day);
            CalculateMonth(year, month);

            IsCalculated = true;
        }

        #region calculating and showing the calendar

        /// <summary>
        /// The main method to show the calendar
        /// This method shows `thisMonth` in `thisYear`
        /// </summary>
        void CalculateMonth(int thisYear, int thisMonth)
        {
            try
            {
                yearForNavigating = thisYear;
                monthForNavigating = thisMonth;
                cal.Clear();

                DateTime tempDateTime = persianCalendar.ToDateTime(yearForNavigating, monthForNavigating, 15, 01, 01, 01, 01);

                int thisDay = 1;
                TextBlockThisMonth.Text = "";

                //Different between first place of calendar and first place of this month
                //اختلاف بین خانه شروع ماه و اولین خانه تقویم            
                string DayOfWeek = persianCalendar.GetDayOfWeek(persianCalendar.ToDateTime(thisYear, thisMonth, 01, 01, 01, 01, 01)).ToString();
                int span = CalculatePersianSpan(DayOfWeek.convertToPersianDay());
                span += 7;
                DecreasePersianDay(ref thisYear, ref thisMonth, ref thisDay, span);

                string persianDate;//حاوی تاریخ روزهای شمسی Contains the date of Persian

                string tooltip_context = "";

                ////////////////////////////////////

                for (int i = -7; i < 7 * 7; i++)
                {
                    tempDateTime = persianCalendar.ToDateTime(thisYear, thisMonth, thisDay, 01, 01, 01, 01);
                    cal.TryAdd(i, tempDateTime);
                    if (i<0 || i > 41)
                    {
                        IncreasePersianDay(ref thisYear, ref thisMonth, ref thisDay, 1);
                        continue;
                    }
                    
                    persianDate = thisDay.ToString(); //.convertToPersianNumber();
                    DayOfWeek = persianCalendar.GetDayOfWeek(tempDateTime).ToString();

                    if (thisMonth == monthForNavigating)
                    {
                        tooltip_context = "";
                        if (thisDay == currentDay && thisMonth == currentMonth && thisYear == currentYear)//بررسی تاریخ امروز Friday
                        {
                            tooltip_context = GetTextOfMemo(thisYear, thisMonth, thisDay, "PERSIAN");


                            if (thisDay == selectedDay && thisMonth == selectedMonth && thisYear == selectedYear)
                            {
                                ChangeProperties(i, persianDate, true, "TextBlockStyle24", tooltip_context);
                            }
                            else if (DayOfWeek == "Friday")//بررسی جمعه بودن روز Friday
                            {
                                ChangeProperties(i, persianDate, true, "TextBlockStyle3", tooltip_context);
                            }
                            else
                            {
                                ChangeProperties(i, persianDate, true, "TextBlockStyle1", tooltip_context);
                            }
                        }
                        else if (SearchInCalendar(thisYear, thisMonth, thisDay, "PERSIAN"))
                        {
                            tooltip_context = GetTextOfMemo(thisYear, thisMonth, thisDay, "PERSIAN");

                            if (thisDay == selectedDay && thisMonth == selectedMonth && thisYear == selectedYear)
                            {
                                ChangeProperties(i, persianDate, false, "TextBlockStyle24", tooltip_context);
                            }
                            else if (isHoliday(thisYear, thisMonth, thisDay, "PERSIAN"))//بررسی جمعه بودن روز Friday
                            {
                                ChangeProperties(i, persianDate, false, "TextBlockStyle3", tooltip_context);
                            }
                            else
                            {
                                ChangeProperties(i, persianDate, false, "TextBlockStyle1", tooltip_context);
                            }
                        }

                        else
                        {
                            if (thisDay == selectedDay && thisMonth == selectedMonth && thisYear == selectedYear)
                            {
                                ChangeProperties(i, persianDate, false, "TextBlockStyle24", tooltip_context);
                            }
                            else if (DayOfWeek == "Friday")//بررسی جمعه بودن روز Friday
                            {
                                ChangeProperties(i, persianDate, false, "TextBlockStyle3", tooltip_context);
                            }
                            else
                            {
                                ChangeProperties(i, persianDate, false, "TextBlockStyle1", tooltip_context);
                            }
                        }
                    }
                    else
                    {
                        if (thisDay == selectedDay && thisMonth == selectedMonth && thisYear == currentYear)
                        {
                            ChangeProperties(i, persianDate, false, "TextBlockStyle24", tooltip_context);
                        }
                        else if (DayOfWeek == "Friday")//بررسی جمعه بودن روز Friday
                        {
                            ChangeProperties(i, persianDate, false, "TextBlockStyle4", tooltip_context);
                        }
                        else
                        {
                            ChangeProperties(i, persianDate, false, "TextBlockStyle2", tooltip_context);
                        }
                    }

                    IncreasePersianDay(ref thisYear, ref thisMonth, ref thisDay, 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// load range of Year
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private static List<int> LoadYear(int year) => Enumerable.Range(year - 50, 100).ToList();

        /// <summary>
        /// اضافه کردن تعداد مشخصی ماه به ورودی
        /// Adds some months (`number`) to the input date
        /// </summary>
        /// <param name="year">سال</param>
        /// <param name="month">ماه</param>
        /// <param name="number">تعداد ماهی که باید به ورودی اضافه شود</param>
        static void IncreasePersianMonth(ref int year, ref int month, int number)
        {
            month += number;
            if (month > 12)
            {
                month = 1;
                year++;
            }
        }

        /// <summary>
        /// کاهش تعداد مشخصی ماه از ورودی
        /// Decreases some months (`number`) from the input date
        /// </summary>
        /// <param name="year">سال</param>
        /// <param name="month">ماه</param>
        /// <param name="number">تعداد ماهی که باید از ورودی کم شود</param>
        static void DecreasePersianMonth(ref int year, ref int month, int number)
        {
            month -= number;
            if (month < 1)
            {
                month = 12;
                year--;
            }
        }

        /// <summary>
        /// اضافه کردن تعداد معلومی روز به ورودی
        /// Adds some days(`number`) to the input date
        /// </summary>
        /// <param name="year">سال</param>
        /// <param name="month">ماه</param>
        /// <param name="day">روز</param>
        /// <param name="number">تعداد روزی که باید به ورودی اضافه شود</param>
        static void IncreasePersianDay(ref int year, ref int month, ref int day, int number)
        {
            int tempDay = day;
            tempDay += number;
            //شش ماه اول سال
            if (month <= 6 && tempDay > 31)
            {
                day = number;
                IncreasePersianMonth(ref year, ref month, 1);
            }
            //5 ماه دوم سال 
            else if (month > 6 && month < 12 && tempDay > 30)
            {
                day = number;
                IncreasePersianMonth(ref year, ref month, 1);
            }
            //اسفند در سال کبیسه
            else if (month == 12 && persianCalendar.IsLeapYear(year) && tempDay > 30)
            {
                day = number;
                IncreasePersianMonth(ref year, ref month, 1);
            }
            //اسفند در سال غیر کبیسه
            else if (month == 12 && !persianCalendar.IsLeapYear(year) && tempDay > 29)
            {
                day = number;
                IncreasePersianMonth(ref year, ref month, 1);
            }
            else
                day += number;
        }

        /// <summary>
        /// کم کردن تعداد معلومی روز از ورودی
        /// Decreases some days (`number`) from the input date
        /// </summary>
        /// <param name="year">سال</param>
        /// <param name="month">ماه</param>
        /// <param name="day">روز</param>
        /// <param name="number">تعداد روزی که باید از ورودی کم شود</param>
        static void DecreasePersianDay(ref int year, ref int month, ref int day, int number)
        {
            int tempDay = day;
            tempDay -= number;
            //شش ماه اول سال
            if (month == 1 && tempDay < 1)
            {
                if (persianCalendar.IsLeapYear(year - 1))
                    day = 30 - number + 1;//+1 رو باید اضافه کرد در غیر این صورت محاسبات اشتباه میشوند ، تجربی
                else
                    day = 29 - number + 1;
                DecreasePersianMonth(ref year, ref month, 1);
            }
            else if (month <= 7 && month > 1 && tempDay < 1)
            {
                day = 31 - number + 1;
                month--;
            }
            //6 ماه دوم سال 
            else if (month > 7 && month <= 12 && tempDay < 1)
            {
                day = 30 - number + 1;
                DecreasePersianMonth(ref year, ref month, 1);
            }
            else
                day -= number;

        }

        /// <summary>
        /// Converts a Persian weekday to equal of it in integer
        /// </summary>
        static int CalculatePersianSpan(string weekday)
        {
            return weekday switch
            {
                "شنبه" => 0,
                "یک شنبه" => 1,
                "دو شنبه" => 2,
                "سه شنبه" => 3,
                "چهار شنبه" => 4,
                "پنج شنبه" => 5,
                "جمعه" => 6,
                _ => 0,
            };
        }

        /// <summary>
        /// Converts the input month number to short equal of it in Christian Calendar
        /// </summary>
        static string EnglishMonthName(int monthNumber)
        {
            return monthNumber switch
            {
                01 => "Jan",
                02 => "Feb",
                03 => "Mar",
                04 => "Apr",
                05 => "May",
                06 => "Jun",
                07 => "Jul",
                08 => "Aug",
                09 => "Sep",
                10 => "Oct",
                11 => "Nov",
                12 => "Dec",
                _ => "",
            };
        }

        /// <summary>
        /// Converts the input persian month name to equal of it in integer
        /// </summary>
        static int NumberOfMonth(string persianMonthName)
        {
            return persianMonthName switch
            {
                "فروردین" => 1,
                "اردیبهشت" => 2,
                "خرداد" => 3,
                "تیر" => 4,
                "مرداد" => 5,
                "شهریور" => 6,
                "مهر" => 7,
                "آبان" => 8,
                "آذر" => 9,
                "دی" => 10,
                "بهمن" => 11,
                "اسفند" => 12,
                _ => 0,
            };
        }

        /// <summary>
        /// Chnages the purpose Grid (`which`) properties with the input data
        /// </summary>
        /// <param name="which">Purpose Grid</param>
        /// <param name="persianDate">Text of Persian date</param>
        /// <param name="persianTextBlockResourceName">New name of Persian date resource</param>
        /// <param name="tooltip_context">Text of tooltip</param>
        void ChangeProperties(int which, string persianDate, bool isCurrentDay, string persianTextBlockResourceName, string tooltip_context)
        {
            if (persianTextBlockResourceName == "TextBlockStyle24")
            {
                selectedBtnIndex = which;
            }

            switch (which)
            {
                case 0:
                    TextBlockShanbe0.Content = persianDate;
                    TextBlockShanbe0.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle5Shanbe2.Style = new Style();
                    if (isCurrentDay) RectangleShanbe0.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") GridShanbe0.ToolTip = tooltip_context;
                    else GridShanbe0.ToolTip = null;
                    break;

                case 1:
                    TextBlock1Shanbe0.Content = persianDate;
                    TextBlock1Shanbe0.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle1Shanbe0.Style = new Style();
                    if (isCurrentDay) Rectangle1Shanbe0.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid1Shanbe0.ToolTip = tooltip_context;
                    else Grid1Shanbe0.ToolTip = null;
                    break;

                case 2:
                    TextBlock2Shanbe0.Content = persianDate;
                    TextBlock2Shanbe0.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle2Shanbe0.Style = new Style();
                    if (isCurrentDay) Rectangle2Shanbe0.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid2Shanbe0.ToolTip = tooltip_context;
                    else Grid2Shanbe0.ToolTip = null;
                    break;

                case 3:
                    TextBlock3Shanbe0.Content = persianDate;
                    TextBlock3Shanbe0.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle3Shanbe0.Style = new Style();
                    if (isCurrentDay) Rectangle3Shanbe0.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid3Shanbe0.ToolTip = tooltip_context;
                    else Grid3Shanbe0.ToolTip = null;
                    break;

                case 4:
                    TextBlock4Shanbe0.Content = persianDate;
                    TextBlock4Shanbe0.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle4Shanbe0.Style = new Style();
                    if (isCurrentDay)
                        Rectangle4Shanbe0.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid4Shanbe0.ToolTip = tooltip_context;
                    else Grid4Shanbe0.ToolTip = null;
                    break;

                case 5:
                    TextBlock5Shanbe0.Content = persianDate;
                    TextBlock5Shanbe0.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle5Shanbe0.Style = new Style();
                    if (isCurrentDay)
                        Rectangle5Shanbe0.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid5Shanbe0.ToolTip = tooltip_context;
                    else Grid5Shanbe0.ToolTip = null;
                    break;

                case 6:
                    TextBlockJome0.Content = persianDate;
                    TextBlockJome0.Style = (Style)FindResource(persianTextBlockResourceName);
                    RectangleJome0.Style = new Style();
                    if (isCurrentDay)
                        RectangleJome0.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") GridJome0.ToolTip = tooltip_context;
                    else GridJome0.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 7:
                    TextBlockShanbe1.Content = persianDate;
                    TextBlockShanbe1.Style = (Style)FindResource(persianTextBlockResourceName);
                    RectangleShanbe1.Style = new Style();
                    if (isCurrentDay)
                        RectangleShanbe1.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") GridShanbe1.ToolTip = tooltip_context;
                    else GridShanbe1.ToolTip = null;
                    break;

                case 8:
                    TextBlock1Shanbe1.Content = persianDate;
                    TextBlock1Shanbe1.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle1Shanbe1.Style = new Style();
                    if (isCurrentDay)
                        Rectangle1Shanbe1.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid1Shanbe1.ToolTip = tooltip_context;
                    else Grid1Shanbe1.ToolTip = null;
                    break;

                case 9:
                    TextBlock2Shanbe1.Content = persianDate;
                    TextBlock2Shanbe1.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle2Shanbe1.Style = new Style();
                    if (isCurrentDay)
                        Rectangle2Shanbe1.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid2Shanbe1.ToolTip = tooltip_context;
                    else Grid2Shanbe1.ToolTip = null;
                    break;

                case 10:
                    TextBlock3Shanbe1.Content = persianDate;
                    TextBlock3Shanbe1.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle3Shanbe1.Style = new Style();
                    if (isCurrentDay)
                        Rectangle3Shanbe1.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid3Shanbe1.ToolTip = tooltip_context;
                    else Grid3Shanbe1.ToolTip = null;
                    break;

                case 11:
                    TextBlock4Shanbe1.Content = persianDate;
                    TextBlock4Shanbe1.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle4Shanbe1.Style = new Style();
                    if (isCurrentDay)
                        Rectangle4Shanbe1.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid4Shanbe1.ToolTip = tooltip_context;
                    else Grid4Shanbe1.ToolTip = null;
                    break;

                case 12:
                    TextBlock5Shanbe1.Content = persianDate;
                    TextBlock5Shanbe1.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle5Shanbe1.Style = new Style();
                    if (isCurrentDay)
                        Rectangle5Shanbe1.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid5Shanbe1.ToolTip = tooltip_context;
                    else Grid5Shanbe1.ToolTip = null;
                    break;

                case 13:
                    TextBlockJome1.Content = persianDate;
                    TextBlockJome1.Style = (Style)FindResource(persianTextBlockResourceName);
                    RectangleJome1.Style = new Style();
                    if (isCurrentDay)
                        RectangleJome1.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") GridJome1.ToolTip = tooltip_context;
                    else GridJome1.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 14:
                    TextBlockShanbe2.Content = persianDate;
                    TextBlockShanbe2.Style = (Style)FindResource(persianTextBlockResourceName);
                    RectangleShanbe2.Style = new Style();
                    if (isCurrentDay)
                        RectangleShanbe2.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") GridShanbe2.ToolTip = tooltip_context;
                    else GridShanbe2.ToolTip = null;
                    break;

                case 15:
                    TextBlock1Shanbe2.Content = persianDate;
                    TextBlock1Shanbe2.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle1Shanbe2.Style = new Style();
                    if (isCurrentDay)
                        Rectangle1Shanbe2.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid1Shanbe2.ToolTip = tooltip_context;
                    else Grid1Shanbe2.ToolTip = null;
                    break;

                case 16:
                    TextBlock2Shanbe2.Content = persianDate;
                    TextBlock2Shanbe2.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle2Shanbe2.Style = new Style();
                    if (isCurrentDay)
                        Rectangle2Shanbe2.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid2Shanbe2.ToolTip = tooltip_context;
                    else Grid2Shanbe2.ToolTip = null;
                    break;

                case 17:
                    TextBlock3Shanbe2.Content = persianDate;
                    TextBlock3Shanbe2.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle3Shanbe2.Style = new Style();
                    if (isCurrentDay)
                        Rectangle3Shanbe2.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid3Shanbe2.ToolTip = tooltip_context;
                    else Grid3Shanbe2.ToolTip = null;
                    break;

                case 18:
                    TextBlock4Shanbe2.Content = persianDate;
                    TextBlock4Shanbe2.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle4Shanbe2.Style = new Style();
                    if (isCurrentDay)
                        Rectangle4Shanbe2.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid4Shanbe2.ToolTip = tooltip_context;
                    else Grid4Shanbe2.ToolTip = null;
                    break;

                case 19:
                    TextBlock5Shanbe2.Content = persianDate;
                    TextBlock5Shanbe2.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle5Shanbe2.Style = new Style();
                    if (isCurrentDay)
                        Rectangle5Shanbe2.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid5Shanbe2.ToolTip = tooltip_context;
                    else Grid5Shanbe2.ToolTip = null;
                    break;

                case 20:
                    TextBlockJome2.Content = persianDate;
                    TextBlockJome2.Style = (Style)FindResource(persianTextBlockResourceName);
                    RectangleJome2.Style = new Style();
                    if (isCurrentDay)
                        RectangleJome2.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") GridJome2.ToolTip = tooltip_context;
                    else GridJome2.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 21:
                    TextBlockShanbe3.Content = persianDate;
                    TextBlockShanbe3.Style = (Style)FindResource(persianTextBlockResourceName);
                    RectangleShanbe3.Style = new Style();
                    if (isCurrentDay)
                        RectangleShanbe3.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") GridShanbe3.ToolTip = tooltip_context;
                    else GridShanbe3.ToolTip = null;
                    break;

                case 22:
                    TextBlock1Shanbe3.Content = persianDate;
                    TextBlock1Shanbe3.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle1Shanbe3.Style = new Style();
                    if (isCurrentDay)
                        Rectangle1Shanbe3.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid1Shanbe3.ToolTip = tooltip_context;
                    else Grid1Shanbe3.ToolTip = null;
                    break;

                case 23:
                    TextBlock2Shanbe3.Content = persianDate;
                    TextBlock2Shanbe3.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle2Shanbe3.Style = new Style();
                    if (isCurrentDay)
                        Rectangle2Shanbe3.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid2Shanbe3.ToolTip = tooltip_context;
                    else Grid2Shanbe3.ToolTip = null;
                    break;

                case 24:
                    TextBlock3Shanbe3.Content = persianDate;
                    TextBlock3Shanbe3.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle3Shanbe3.Style = new Style();
                    if (isCurrentDay)
                        Rectangle3Shanbe3.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid3Shanbe3.ToolTip = tooltip_context;
                    else Grid3Shanbe3.ToolTip = null;
                    break;

                case 25:
                    TextBlock4Shanbe3.Content = persianDate;
                    TextBlock4Shanbe3.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle4Shanbe3.Style = new Style();
                    if (isCurrentDay)
                        Rectangle4Shanbe3.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid4Shanbe3.ToolTip = tooltip_context;
                    else Grid4Shanbe3.ToolTip = null;
                    break;

                case 26:
                    TextBlock5Shanbe3.Content = persianDate;
                    TextBlock5Shanbe3.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle5Shanbe3.Style = new Style();
                    if (isCurrentDay)
                        Rectangle5Shanbe3.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid5Shanbe3.ToolTip = tooltip_context;
                    else Grid5Shanbe3.ToolTip = null;
                    break;

                case 27:
                    TextBlockJome3.Content = persianDate;
                    TextBlockJome3.Style = (Style)FindResource(persianTextBlockResourceName);
                    RectangleJome3.Style = new Style();
                    if (isCurrentDay)
                        RectangleJome3.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") GridJome3.ToolTip = tooltip_context;
                    else GridJome3.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 28:
                    TextBlockShanbe4.Content = persianDate;
                    TextBlockShanbe4.Style = (Style)FindResource(persianTextBlockResourceName);
                    RectangleShanbe4.Style = new Style();
                    if (isCurrentDay)
                        RectangleShanbe4.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") GridShanbe4.ToolTip = tooltip_context;
                    else GridShanbe4.ToolTip = null;
                    break;

                case 29:
                    TextBlock1Shanbe4.Content = persianDate;
                    TextBlock1Shanbe4.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle1Shanbe4.Style = new Style();
                    if (isCurrentDay)
                        Rectangle1Shanbe4.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid1Shanbe4.ToolTip = tooltip_context;
                    else Grid1Shanbe4.ToolTip = null;
                    break;

                case 30:
                    TextBlock2Shanbe4.Content = persianDate;
                    TextBlock2Shanbe4.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle2Shanbe4.Style = new Style();
                    if (isCurrentDay)
                        Rectangle2Shanbe4.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid2Shanbe4.ToolTip = tooltip_context;
                    else Grid2Shanbe4.ToolTip = null;
                    break;

                case 31:
                    TextBlock3Shanbe4.Content = persianDate;
                    TextBlock3Shanbe4.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle3Shanbe4.Style = new Style();
                    if (isCurrentDay)
                        Rectangle3Shanbe4.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid3Shanbe4.ToolTip = tooltip_context;
                    else Grid3Shanbe4.ToolTip = null;
                    break;

                case 32:
                    TextBlock4Shanbe4.Content = persianDate;
                    TextBlock4Shanbe4.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle4Shanbe4.Style = new Style();
                    if (isCurrentDay)
                        Rectangle4Shanbe4.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid4Shanbe4.ToolTip = tooltip_context;
                    else Grid4Shanbe4.ToolTip = null;
                    break;

                case 33:
                    TextBlock5Shanbe4.Content = persianDate;
                    TextBlock5Shanbe4.Style = (Style)FindResource(persianTextBlockResourceName);
                    Rectangle5Shanbe4.Style = new Style();
                    if (isCurrentDay)
                        Rectangle5Shanbe4.Style = (Style)FindResource("RectangleStyleToday");
                    if (tooltip_context != "") Grid5Shanbe4.ToolTip = tooltip_context;
                    else Grid5Shanbe4.ToolTip = null;
                    break;

                case 34:
                    TextBlockJome4.Content = persianDate;
                    TextBlockJome4.Style = (Style)FindResource(persianTextBlockResourceName);
                    RectangleJome4.Style = new Style();
                    if (isCurrentDay)
                        RectangleJome4.Style = (Style)FindResource("RectangleStyleToday");

                    if (tooltip_context != "") GridJome4.ToolTip = tooltip_context;
                    else GridJome4.ToolTip = null;
                    break;

                ///////////////////////////////////////////

                case 35:
                    TextBlockShanbe5.Content = persianDate;
                    TextBlockShanbe5.Style = (Style)FindResource(persianTextBlockResourceName);
                    if (tooltip_context != "") GridShanbe5.ToolTip = tooltip_context;
                    else GridShanbe5.ToolTip = null;
                    break;

                case 36:
                    TextBlock1Shanbe5.Content = persianDate;
                    TextBlock1Shanbe5.Style = (Style)FindResource(persianTextBlockResourceName);
                    if (tooltip_context != "") Grid1Shanbe5.ToolTip = tooltip_context;
                    else Grid1Shanbe5.ToolTip = null;
                    break;

                case 37:
                    TextBlock2Shanbe5.Content = persianDate;
                    TextBlock2Shanbe5.Style = (Style)FindResource(persianTextBlockResourceName);
                    if (tooltip_context != "") Grid2Shanbe5.ToolTip = tooltip_context;
                    else Grid2Shanbe5.ToolTip = null;
                    break;

                case 38:
                    TextBlock3Shanbe5.Content = persianDate;
                    TextBlock3Shanbe5.Style = (Style)FindResource(persianTextBlockResourceName);
                    if (tooltip_context != "") Grid3Shanbe5.ToolTip = tooltip_context;
                    else Grid3Shanbe5.ToolTip = null;
                    break;

                case 39:
                    TextBlock4Shanbe5.Content = persianDate;
                    TextBlock4Shanbe5.Style = (Style)FindResource(persianTextBlockResourceName);
                    if (tooltip_context != "") Grid4Shanbe5.ToolTip = tooltip_context;
                    else Grid4Shanbe5.ToolTip = null;
                    break;

                case 40:
                    TextBlock5Shanbe5.Content = persianDate;
                    TextBlock5Shanbe5.Style = (Style)FindResource(persianTextBlockResourceName);
                    if (tooltip_context != "") Grid5Shanbe5.ToolTip = tooltip_context;
                    else Grid5Shanbe5.ToolTip = null;
                    break;

                case 41:
                    TextBlockJome5.Content = persianDate;
                    TextBlockJome5.Style = (Style)FindResource(persianTextBlockResourceName);
                    if (tooltip_context != "") GridJome5.ToolTip = tooltip_context;
                    else GridJome5.ToolTip = null;
                    break;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// نمایش ماه بعد
        /// Shows the next month
        /// </summary>
        void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            IncreasePersianMonth(ref yearForNavigating, ref monthForNavigating, 1);
            CalculateMonth(yearForNavigating, monthForNavigating);
            this.comboBoxMonths.SelectedIndex = monthForNavigating - 1;
            this.comboBoxYear.SelectedItem = yearForNavigating;
        }

        /// <summary>
        /// نمایش ماه قبل
        /// Shows the previous month
        /// </summary>
        void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            DecreasePersianMonth(ref yearForNavigating, ref monthForNavigating, 1);
            CalculateMonth(yearForNavigating, monthForNavigating);
            this.comboBoxMonths.SelectedIndex = monthForNavigating - 1;
            this.comboBoxYear.SelectedItem = yearForNavigating;
        }

        /// <summary>
        /// پرش به تاریخ امروز
        /// Shows the month of today
        /// </summary>
        void GoToToday_Click(object sender, RoutedEventArgs e)
        {
            CalculateMonth(this.currentYear, this.currentMonth);
        }

        private void ComboBoxMonths_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsCalculated)
            {
                return;
            }
            CalculateMonth(yearForNavigating, comboBoxMonths.SelectedIndex + 1);
        }

        private void ComboBoxYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsCalculated)
            {
                return;
            }
            CalculateMonth((int)comboBoxYear.SelectedItem, monthForNavigating);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = (sender as Button).TabIndex;
            var date = cal[index];
            SelectedDate = date;
            Click?.Invoke(this, e);
        }
        #endregion Events

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var _FrameworkElement = e.OriginalSource as FrameworkElement;
            if (e.Key == Key.Left)
            {
                var date = SetSelectedBtnIndex(1);
                SelectedDate = date;
                return;
            }
            if (e.Key == Key.Right)
            {
                var date = SetSelectedBtnIndex(-1);
                SelectedDate = date;
                return;
            }
            if (e.Key == Key.Up)
            {
                var date = SetSelectedBtnIndex(-7);
                SelectedDate = date;
                return;
            }
            if (e.Key == Key.Down)
            {
                var date = SetSelectedBtnIndex(7);
                SelectedDate = date;
                return;
            }
            if (e.Key == Key.Space)
            {
                Click?.Invoke(this, e);
                return;
            }

            MKeyDown?.Invoke(this, e);
        }

        private DateTime SetSelectedBtnIndex(int v)
        {
            selectedBtnIndex += v;
            return cal[selectedBtnIndex];
        }
    }


}
