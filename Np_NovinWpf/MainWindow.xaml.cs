using System.Windows;
using System.Windows.Controls;

namespace Np_NovinWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Click_Me(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string str = btn.Content.ToString() + " button clicked";
            MessageBox.Show(str);
        }
    }
}
