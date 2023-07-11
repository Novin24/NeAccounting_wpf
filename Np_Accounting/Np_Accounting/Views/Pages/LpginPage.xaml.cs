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

namespace Np_Accounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for LpginPage.xaml
    /// </summary>
    public partial class LpginPage : Page
    {
        public LpginPage()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.LeftButton == MouseButtonStte.Pressed)

            // DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            //WindoState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.Shutdown();
        }

        private void btnlogin_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
