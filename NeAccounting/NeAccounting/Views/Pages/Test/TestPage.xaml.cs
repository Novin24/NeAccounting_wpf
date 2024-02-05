using NeAccounting.ViewModels;
using System.Windows.Media;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages.Test
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : INavigableView<TestViewModel>
    {
        private int click;

        public TestViewModel ViewModel{ get;}

        public TestPage(TestViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            click = 0;
        }

        //private void btn_font_Click(object sender, RoutedEventArgs e)
        //{
        //    click++;
        //    txt.Text = click.ToString();

        //}
    }
}
