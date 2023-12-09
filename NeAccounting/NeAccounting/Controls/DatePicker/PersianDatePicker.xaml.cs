using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace NeAcconting.Controls.DatePicker
{
    [DefaultEvent("SelectedDateChanged")]
    [DefaultProperty("SelectedDate")]
    public partial class PersianDatePicker : UserControl
    {
        public static readonly DependencyProperty TextProperty;

        public static readonly DependencyProperty SelectedDateProperty;

        public static readonly DependencyProperty DisplayDateProperty;

        public static readonly DependencyProperty DisplayDateStartProperty;

        public static readonly DependencyProperty DisplayDateEndProperty;

        public static readonly RoutedEvent SelectedDateChangedEvent;

        public PersianDatePicker()
        {
            InitializeComponent();
        }

        [Category("Date Picker")]
        public DateTime SelectedDate
        {
            get { return persianCalendar.SelectedDate; }
            set { persianCalendar.SelectedDate = value; }
        }

        /// <summary>
        /// Gets or sets the date that is being displayed in the calendar.
        /// </summary>
        [Category("Date Picker")]
        public string DisplayDate
        {
            get { return Text; }
            set { Text = value; }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        //events
        public event RoutedEventHandler SelectedDateChanged
        {
            add { AddHandler(SelectedDateChangedEvent, value); }
            remove { RemoveHandler(SelectedDateChangedEvent, value); }
        }


        static void selectedDateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            PersianDatePicker pdp = o as PersianDatePicker;
            pdp.Text = e.NewValue.ToString();
            pdp.RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent, pdp));
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            persianCalnedarPopup.IsOpen = true;
        }



        private void persianCalnedarPopup_Opened(object sender, EventArgs e)
        {
            this.persianCalendar.Focus();
        }
    }
}
