using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for FinancialAidListPage.xaml
    /// </summary>
    public partial class FinancialAidListPage : INavigableView<AidListViewModel>
    {
        public AidListViewModel ViewModel { get; }
        public FinancialAidListPage(AidListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_name.Focus();
        }
    }
}
