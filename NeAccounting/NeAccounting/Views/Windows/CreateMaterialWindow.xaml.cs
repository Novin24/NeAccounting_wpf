using NeAccounting.ViewModels;

namespace NeAccounting.Windows
{
    /// <summary>
    /// Interaction logic for CreateMaterialWindow.xaml
    /// </summary>
    public partial class CreateMaterialWindow 
    {
        public HotCreateMaterailViewModel ViewModel { get; }

        public CreateMaterialWindow(HotCreateMaterailViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        [RelayCommand]
        private void OnClose() => Visibility = Visibility.Hidden;

        [RelayCommand]
        private async Task OnCreateMat()
        {
            if (DataContext is not CreateMaterialWindow cmw)
            {
                return;
            }

          await cmw.ViewModel.CreateMatCommand.ExecuteAsync(this);
        }
    }
}
