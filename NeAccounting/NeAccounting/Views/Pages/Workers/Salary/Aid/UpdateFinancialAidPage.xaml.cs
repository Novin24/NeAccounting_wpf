using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateFinancialAidPage.xaml
    /// </summary>
    public partial class UpdateFinancialAidPage : INavigableView<UpdateFinancialAidViewModel>
    {
        public UpdateFinancialAidViewModel ViewModel { get; }

        public UpdateFinancialAidPage(UpdateFinancialAidViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
