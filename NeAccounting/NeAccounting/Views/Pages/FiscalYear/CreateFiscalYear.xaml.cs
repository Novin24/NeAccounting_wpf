using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateFiscalYear.xaml
    /// </summary>
    public partial class CreateFiscalYear : INavigableView<CreateFinancialViewModel>
    {
        public CreateFinancialViewModel ViewModel { get; }
        public CreateFiscalYear(CreateFinancialViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
