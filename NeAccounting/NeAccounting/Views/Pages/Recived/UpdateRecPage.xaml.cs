using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for eRecPagePage1.xaml
    /// </summary>
    public partial class UpdateRecDocPage : INavigableView<UpdateRecDocViewModel>
    {
        public UpdateRecDocViewModel ViewModel { get; }

        public UpdateRecDocPage(UpdateRecDocViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
