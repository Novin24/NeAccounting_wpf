using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages.Test
{
    /// <summary>
    /// Interaction logic for TestPage.xaml
    /// </summary>
    public partial class TestPage : INavigableView<TestViewModel>
    {
        public TestViewModel ViewModel{ get;}
        public TestPage(TestViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void prices_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(prices.Text))
            {
                //System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                //var valueBefore = Int64.Parse(prices.Text, System.Globalization.NumberStyles.AllowThousands);
                //prices.Text = string.Format(culture, "{0:N0}", valueBefore);
                //prices.Select(prices.Text.Length, 0);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = prices.Text;
        }
    }
}
