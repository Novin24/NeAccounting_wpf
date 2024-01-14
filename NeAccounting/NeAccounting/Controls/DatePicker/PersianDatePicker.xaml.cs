using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace NeAcconting.Controls.DatePicker
{
    [DefaultEvent("SelectedDateChanged")]
    [DefaultProperty("SelectedDate")]
    public partial class PersianDatePicker : UserControl
    {
        public PersianDatePicker()
        {
            InitializeComponent();
            dateTextBox.Text = persianCalendar.PersianSelectedDate;
        }


        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set
            {
                SetValue(SelectedDateProperty, value);
            }
        }

        // Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(PersianDatePicker), new PropertyMetadata(DateTime.Now));
        public string DisplayDate
        {
            get { return (string)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(string), typeof(PersianDatePicker), new PropertyMetadata("... تاریخ امروز",SetDate));

        private static void SetDate(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is not PersianDatePicker pdp)
                return;

            if (args.NewValue.ToString() == args.OldValue.ToString())
                return;

           pdp.dateTextBox.Text = args.NewValue.ToString();
        }

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
            //dateTextBox.Text = persianCalendar.PersianSelectedDate;
            dateTextBox.Focus();
        }

        private void PersianCalendar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //dateTextBox.Text = persianCalendar.PersianSelectedDate;
        }
    }
}
