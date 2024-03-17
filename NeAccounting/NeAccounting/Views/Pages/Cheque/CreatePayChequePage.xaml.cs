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
    /// Interaction logic for CreatePayCheque.xaml
    /// </summary>
    public partial class CreatePayChequePage : Page
    {
        public CreatePayChequePage()
        {
            //ViewModel = viewModel;
            //DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_Name.Focus();
        }
    }
}
