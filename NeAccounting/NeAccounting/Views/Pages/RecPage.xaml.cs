using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class RecPage : INavigableView<RecViewModel>
    {
        public RecViewModel ViewModel { get; }

        public RecPage(RecViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
