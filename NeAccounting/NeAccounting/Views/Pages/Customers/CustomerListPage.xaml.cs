using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class CustomerListPage : INavigableView<CustomerListViewModel>
    {
        public CustomerListViewModel ViewModel { get; }
        public CustomerListPage(CustomerListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            CusName_txt.Focus();
        }
    }
}
