using System.ComponentModel;
using System.Windows.Controls;

namespace NeAcconting.Controls.DatePicker
{
    [DefaultEvent("SelectedDateChanged")]
    [DefaultProperty("SelectedDate")]
    public partial class PersianDatePicker : UserControl
    {


        public PersianDatePicker()
        {
            InitializeComponent();
            //dateTextBox.Text = PersianSelectedDate;
            //persianCalendar.Click += (object sender, RoutedEventArgs e) =>
            //{
            //    this.persianCalnedarPopup.IsOpen = false;
            //};
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
            get
            {
                var s = (string)GetValue(DisplayDateProperty);
                return s;
            }
            set { SetValue(DisplayDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(string), typeof(PersianDatePicker), new PropertyMetadata(string.Empty));


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            persianCalnedarPopup.IsOpen = true;
        }

        private void persianCalnedarPopup_Opened(object sender, EventArgs e)
        {
            this.persianCalendar.Focus();
        }

        private void persianCalendar_Click(object sender, RoutedEventArgs e)
        {
            persianCalnedarPopup.IsOpen = false;
            dateTextBox.Text = persianCalendar.PersianSelectedDate;
        }
    }
}
