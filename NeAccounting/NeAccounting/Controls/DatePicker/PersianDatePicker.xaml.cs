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
            //dateTextBox.Text = persianCalendar.PersianSelectedDate;
            persianCalendar.Click += (object sender, RoutedEventArgs e) =>
            {
                this.persianCalnedarPopup.IsOpen = false;
                //dateTextBox.Text = persianCalendar.PersianSelectedDate;
            };
        }

        //[Category("Date Picker")]
        //public DateTime SelectedDate
        //{
        //    get { return persianCalendar.SelectedDate; }
        //    set
        //    {
        //        persianCalendar.SelectedDate = value;
        //        dateTextBox.Text = persianCalendar.PersianSelectedDate;
        //    }
        //}



        public DateTime SelectedDate
        {
            get { return (DateTime)GetValue(SelectedDateProperty); }
            set
            { SetValue(SelectedDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime), typeof(PersianDatePicker), new PropertyMetadata(DateTime.Now));

        //private static void SelectedDayChenged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        //{
        //    if (obj is not PersianDatePicker ps)
        //        return;
        //    var t = (DateTime)args.NewValue;
        //    ps.SetDateBox();
        //}

        //private void SetDateBox()
        //{ 
        //    dateTextBox.Text = persianCalendar.PersianSelectedDate;
        //}

        /// <summary>
        /// Gets or sets the date that is being displayed in the calendar.
        /// </summary>
        //[Category("Date Picker")]
        //public string DisplayDate
        //{
        //    get { return dateTextBox.Text; }
        //    set { dateTextBox.Text = value; }
        //}



        public string DisplayDate
        {
            get { return (string)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(string), typeof(PersianDatePicker), new PropertyMetadata(null));


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
