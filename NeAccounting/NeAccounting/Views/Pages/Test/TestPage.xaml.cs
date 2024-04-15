using NeAccounting.ViewModels;

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



        //private void btn_font_Click(object sender, RoutedEventArgs e)
        //{
        //    click++;
        //    txt.Text = click.ToString();

        //}
    }
}
