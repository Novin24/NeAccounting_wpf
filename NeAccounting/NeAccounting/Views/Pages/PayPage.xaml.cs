using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for PayPage.xaml
    /// </summary>
    public partial class PayPage : INavigableView<ViewModels.PayViewModel>
    {
        public PayViewModel ViewModel { get; }

        public PayPage(PayViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
