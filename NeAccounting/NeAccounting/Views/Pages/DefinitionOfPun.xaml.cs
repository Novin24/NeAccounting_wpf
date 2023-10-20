using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DefinitionOfPun.xaml
    /// </summary>
    public partial class DefinitionOfPun : INavigableView<PayViewModel>
    {
        public PayViewModel ViewModel { get; }
        public DefinitionOfPun(PayViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

    }
}
