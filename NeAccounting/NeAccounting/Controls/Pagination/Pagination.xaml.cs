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

        protected virtual void InitialPage(int currentPage)
        {
            CalculatePages(PageCount, currentPage);
        }

        private void CalculatePages(int pageCount, int currentPage)
        {
            btn_1.Visibility = Visibility.Collapsed;
            btn_2.Visibility = Visibility.Collapsed;
            btn_3.Visibility = Visibility.Collapsed;
            btn_4.Visibility = Visibility.Collapsed;

            if (currentPage > 1)
            {
                btn_back.Tag = currentPage - 1;
                btn_back.Visibility = Visibility.Visible;
            }
            else
            {
                btn_back.Visibility = Visibility.Collapsed;
            }


            int b = 1;
            for (int i = 1; i < pageCount + 1; i++)
            {
                if (pageCount >= 5 && i == currentPage - 3)
                {
                    btn_le.Visibility = Visibility.Visible;
                    continue;
                }
                else if (i == currentPage)
                {
                    if (pageCount > 10)
                    {
                        btn_mid.Visibility = Visibility.Collapsed;
                        txt_mid.Visibility = Visibility.Visible;
                        txt_mid.Value = i;
                    }
                    else
                    {
                        btn_mid.Visibility = Visibility.Visible;
                        txt_mid.Visibility = Visibility.Collapsed;
                        btn_mid.Content = i;
                    }
                    continue;
                }
                else if (i < currentPage + 3 && i > currentPage - 3)
                {
                    if (b == 1)
                    {
                        btn_1.Visibility = Visibility.Visible;
                        btn_1.Content = i;
                        btn_1.Tag = i;
                        b++;
                        continue;
                    }
                    if (b == 2)
                    {
                        btn_2.Visibility = Visibility.Visible;
                        btn_2.Content = i;
                        btn_2.Tag = i;
                        b++;
                        continue;
                    }
                    if (b == 3)
                    {
                        btn_3.Visibility = Visibility.Visible;
                        btn_3.Content = i;
                        btn_3.Tag = i;
                        b++;
                        continue;
                    }
                    if (b == 4)
                    {
                        btn_4.Visibility = Visibility.Visible;
                        btn_4.Content = i;
                        btn_4.Tag = i;
                        b++;
                        continue;
                    }
                }
                else if (pageCount >= 5 && i == currentPage + 3)
                {
                    btn_ri.Visibility = Visibility.Visible;
                    continue;
                }
            }


            if (currentPage < pageCount)
            {
                btn_forward.Tag = currentPage + 1;
                btn_forward.Visibility = Visibility.Visible;
            }
            else
            {
                btn_forward.Visibility = Visibility.Collapsed;
            }
        }

        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minpage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(Pagination), new PropertyMetadata(0));

        public int CurrntPage
        {
            get { return (int)GetValue(CurrntPageProperty); }
            set { SetValue(CurrntPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Maxpage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrntPageProperty =
            DependencyProperty.Register("CurrntPage", typeof(int), typeof(Pagination), new PropertyMetadata(0, PropertyChenged));


        private static void PropertyChenged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is not Pagination pg)
                return;

            if (args.NewValue == args.OldValue)
                return;

            pg.InitialPage((int)args.NewValue);
        }


    }
}
