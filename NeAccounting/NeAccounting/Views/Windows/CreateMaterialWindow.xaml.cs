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
            MaterialName.Focus();
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

        private async void FluentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeViewModelCommand.ExecuteAsync(null);
        }
    }
}
