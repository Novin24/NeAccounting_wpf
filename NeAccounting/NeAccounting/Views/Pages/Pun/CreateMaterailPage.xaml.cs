using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DefinitionOfPun.xaml
    /// </summary>
    public partial class CreateMaterailPage : INavigableView<CreateMaterailViewModel>
    {
        public CreateMaterailViewModel ViewModel { get; }

        public CreateMaterailPage(CreateMaterailViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            MaterialName.Focus();
        }

        [RelayCommand]
        private async Task OnCreateMaterial()
        {
            Btn_submit.Focus();
            await ViewModel.CreateMaterialCommand.ExecuteAsync(null);
        }
    }
}
