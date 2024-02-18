using System.Windows.Controls;

namespace NeAccounting.Controls
{
    /// <summary>
    /// Interaction logic for Pagination.xaml
    /// </summary>
    public partial class Pagination : UserControl
    {
        public Pagination()
        {
            InitializeComponent();
        }


        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minpage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(Pagination), new PropertyMetadata(1));



        public int Maxpage
        {
            get { return (int)GetValue(MaxpageProperty); }
            set { SetValue(MaxpageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maxpage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxpageProperty =
            DependencyProperty.Register("Maxpage", typeof(int), typeof(Pagination), new PropertyMetadata(0));


    }
}
