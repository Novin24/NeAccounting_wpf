using DomainShared.ViewModels;
using NeAccounting.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DefinitionOfPun.xaml
    /// </summary>
    public partial class CreateMaterailPage : INavigableView<CreateMaterailViewModel>
    {
        public CreateMaterailViewModel ViewModel { get; }
        public int materailId { get; set; }


        public CreateMaterailPage(CreateMaterailViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

    }
}
