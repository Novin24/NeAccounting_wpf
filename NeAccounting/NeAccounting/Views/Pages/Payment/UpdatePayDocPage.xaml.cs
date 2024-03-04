using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdatePayPage.xaml
    /// </summary>
    public partial class UpdatePayDocPage : INavigableView<UpdatePayDocViewModel>
    {
        public UpdatePayDocViewModel ViewModel { get; }

        public UpdatePayDocPage(UpdatePayDocViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
