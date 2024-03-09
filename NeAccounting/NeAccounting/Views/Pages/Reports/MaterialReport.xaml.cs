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

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for MaterialReport.xaml
    /// </summary>
    public partial class MaterialReport : Page
    {
        public MaterialReport()
        {
            InitializeComponent();
        }

        private void Pagination_PageChosen(object sender, RoutedPropertyChangedEventArgs<int> e)
        {

        }
    }
}
