using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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


        public int Minpage
        {
            get { return (int)GetValue(MinpageProperty); }
            set { SetValue(MinpageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Minpage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinpageProperty =
            DependencyProperty.Register("Minpage", typeof(int), typeof(Pagination), new PropertyMetadata(0));



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
