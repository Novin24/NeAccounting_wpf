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
            dateTextBox.Text = persianCalendar.PersianSelectedDate;
            persianCalendar.Click += (object sender, RoutedEventArgs e) =>
            {
                this.persianCalnedarPopup.IsOpen = false;
                dateTextBox.Text = persianCalendar.PersianSelectedDate;
            };
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
            get { return dateTextBox.Text; }
            set { dateTextBox.Text = value; }
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
