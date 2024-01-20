using System.ComponentModel;
using System.Windows.Controls;

namespace NeAccounting.Controls
{
    [DefaultEvent("SelectedDateChanged")]
    [DefaultProperty("SelectedDate")]
    public partial class PersianDatePicker : UserControl
    {
        public PersianDatePicker()
        {
            InitializeComponent();
            txt_date.Text = persianCalendar.PersianSelectedDate;
        }

        #region LableName
        public string LabelName
        {
            get { return (string)GetValue(LableNameProperty); }
            set { SetValue(LableNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LableName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LableNameProperty =
            DependencyProperty.Register("LableName", typeof(string), typeof(PersianDatePicker), new PropertyMetadata(string.Empty, SetLabelName));

        private static void SetLabelName(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not PersianDatePicker npack)
                return;

            if (e.NewValue == e.OldValue)
                return;

            npack.lbl_name.Text = e.NewValue.ToString();
        }
        #endregion

        #region SelectedDate
        /// <summary>
        /// تاریخ انتخاب شده
        /// </summary>
        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set
            {
                SetValue(SelectedDateProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", propertyType: typeof(DateTime?), typeof(PersianDatePicker), new PropertyMetadata(null));
        #endregion

        #region DisplayDate

        /// <summary>
        /// تاریخ قابل نمایش فارسی
        /// </summary>
        public string DisplayDate
        {
            get { return (string)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(string), typeof(PersianDatePicker), new PropertyMetadata(string.Empty,SetDate));

        private static void SetDate(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is not PersianDatePicker pdp)
                return;

            if (args.NewValue == args.OldValue)
                return;

           pdp.txt_date.Text = args.NewValue.ToString();
        }
        #endregion

        #region Event
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            persianCalnedarPopup.IsOpen = true;
        }

        private void PersianCalnedarPopup_Opened(object sender, EventArgs e)
        {
            this.persianCalendar.Focus();
        }

        private void PersianCalendar_Click(object sender, RoutedEventArgs e)
        {
            persianCalnedarPopup.IsOpen = false;
            txt_date.Focus();
        }

        private void Dismiss_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate = null;
            txt_date.Clear();
        }
        #endregion
    }
}
