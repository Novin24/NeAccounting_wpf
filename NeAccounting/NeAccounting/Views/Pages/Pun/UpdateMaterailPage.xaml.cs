using NeAccounting.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for EditPunPage.xaml
    /// </summary>
    public partial class UpdateMaterailPage : INavigableView<UpdateMaterailViewModel>
    {
        public UpdateMaterailViewModel ViewModel { get; }

        public UpdateMaterailPage(UpdateMaterailViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
