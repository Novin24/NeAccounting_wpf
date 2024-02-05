using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateSellInvoicePage.xaml
    /// </summary>
    public partial class CreateSellInvoicePage : INavigableView<CreateSellInviceViewModel>
    {
        public CreateSellInviceViewModel ViewModel { get; }
        public CreateSellInvoicePage(CreateSellInviceViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.OnAdd())
            {
                ViewModel.TotalPrice = 0;
                ViewModel.AmountOf = 0;
                ViewModel.MaterialId = -1;
                txt_MaterialName.Text = string.Empty;
                txt_amount.Value = 0;
                txt_description.Text = string.Empty;
            }
        }
    }
}
