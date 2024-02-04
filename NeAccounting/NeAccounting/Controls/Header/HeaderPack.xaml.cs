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
    }
}
