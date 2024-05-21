using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for ProfitOrLessPage.xaml
    /// </summary>
    public partial class ProfitOrLessPage : INavigableView<ProfitOrLessViewModel>
    {
        public ProfitOrLessViewModel ViewModel { get; }

        public ProfitOrLessPage(ProfitOrLessViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_Profit.Focus();
        }
    }
}
