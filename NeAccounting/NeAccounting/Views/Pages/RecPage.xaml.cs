using Wpf.Ui.Controls;

namespace NeAccounting.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class RecPage : INavigableView<ViewModels.RecViewModel>
    {
        public ViewModels.RecViewModel ViewModel { get; }

        public RecPage(ViewModels.RecViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
