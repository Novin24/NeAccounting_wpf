using NeAccounting.ViewModels;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// صفحه نمایش اجناس
    /// </summary>
    public partial class MaterailListPage :  INavigableView<MaterailListViewModel>
    {
        private readonly IContentDialogService _contentDialogService;
        public MaterailListViewModel ViewModel { get; }

        public MaterailListPage(IContentDialogService contentDialogService, MaterailListViewModel viewModel)
        {
            _contentDialogService = contentDialogService;
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            materialName_txb.Focus();
        }

        private void CheckBox_Status_Chkecked(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.CheckBox btn)
                return;

            if (btn.Tag == null)
                return;
            int id = int.Parse(btn.Tag.ToString());
            var unit = ViewModel.List.First(x => x.Id == id);
            if (unit.IsActive)
            {
                return;
            }
            ViewModel.ActiveCommand.ExecuteAsync(id);
        }
        private async void CheckBox_Status_Unckecked(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.CheckBox btn)
                return;
            btn.IsChecked = true;

            if (btn.Tag == null)
                return;

            int id = int.Parse(btn.Tag.ToString());
            var unit = ViewModel.List.First(x => x.Id == id);
            if (!unit.IsActive)
            {
                return;
            }
            var result = await _contentDialogService.ShowSimpleDialogAsync(new SimpleContentDialogCreateOptions()
            {
                Title = "آیا از بایگانی اطمینان دارید!!!",
                Content = Application.Current.Resources["DeleteDialogContent"],
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });

            if (result == ContentDialogResult.Primary)
            {
            ViewModel.DeActiveCommand.ExecuteAsync(id);
            }
        }

    }
}
