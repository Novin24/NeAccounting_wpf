using System.Globalization;
using System.Windows.Controls;

namespace NeAccounting.Controls
{
    /// <summary>
    /// Interaction logic for HeaderPack.xaml
    /// </summary>
    public partial class HeaderPack : UserControl
    {

        public HeaderPack()
        {
            InitializeComponent();

            PersianCalendar persianCalendar = new PersianCalendar();
            string DateHeader = persianCalendar.GetYear(DateTime.Now) + "/" + persianCalendar.GetMonth(DateTime.Now) + "/" + persianCalendar.GetDayOfMonth(DateTime.Now);
            txt_date.Text = DateHeader;
        }


        public string NamePpage
        {
            get { return (string)GetValue(NamePpageProperty); }
            set { SetValue(NamePpageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NamePpage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NamePpageProperty =
            DependencyProperty.Register("NamePpage", typeof(string), typeof(HeaderPack), new PropertyMetadata(string.Empty, SetNamePpage));
        private static void SetNamePpage(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is not HeaderPack hp)
                return;

            if (e.NewValue == e.OldValue)
                return;

            hp.page_name.Text = e.NewValue.ToString();

        }
    }
}
