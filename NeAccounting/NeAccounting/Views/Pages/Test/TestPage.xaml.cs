using NeAccounting.ViewModels;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages.Test
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage// : INavigableView<TestViewModel>
    {
        //public TestViewModel ViewModel { get; }

        public TestPage(TestViewModel viewModel)
        {
            //ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> list = [];
            md_newModal.IsOpen = false;
            foreach (var el in sp_total.Children)
            {
                if (el is not TextBox txb)
                {
                    continue;
                }
                list.Add(txb.Text);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            md_newModal.IsOpen = true;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            sp_total.Children.Add(new TextBox() { Text = "ffffffffffff", Foreground = new SolidColorBrush(Colors.Black) });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            sp_total.Children.Clear();
        }




        //private void btn_font_Click(object sender, RoutedEventArgs e)
        //{
        //    click++;
        //    txt.Text = click.ToString();

        //}
    }
}
