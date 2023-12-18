using System.Windows.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateCustomerPage.xaml
    /// </summary>
    public partial class CreateCustomerPage : Page
    {
        public CreateCustomerPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = txt_CashGrantee.Value;
            var c = txt_Grantee.Value;
        }
    }
}
