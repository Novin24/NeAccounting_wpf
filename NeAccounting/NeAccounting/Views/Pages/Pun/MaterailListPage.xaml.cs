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

        private async void CheckBox_Status_Chkecked(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.CheckBox btn)
                return;

            if (btn.Tag == null)
                return;
            Guid id = Guid.Parse(btn.Tag.ToString());
            var unit = ViewModel.List.First(x => x.Id == id);
            if (unit.IsActive)
            {
                return;
            }
           await ViewModel.ActiveCommand.ExecuteAsync(id);
        }
        private async void CheckBox_Status_Unckecked(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.CheckBox btn)
                return;
            btn.IsChecked = true;

            if (btn.Tag == null)
                return;

            Guid id = Guid.Parse(btn.Tag.ToString());
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
            await ViewModel.DeActiveCommand.ExecuteAsync(id);
            }
        }
		private void OnMinimumStockClick(object sender, RoutedEventArgs e)
		{
			// باز کردن Flyout با استفاده از x:Name
			MinimumStockFlyout.IsOpen = true;
		}
		private async void ConfirmMinimumStockClick(object sender, RoutedEventArgs e)
		{
			// گرفتن مقدار از TextBox
			var miniStockValue = MinimumStockTextBox.Text;

			// فرض کنید CommandParameter از اطلاعات درست استفاده می‌کند
			if (Guid.TryParse((sender as Button).CommandParameter.ToString(), out Guid id))
			{
				await ViewModel.ChangeMiniEntityCommand(id, int.Parse(miniStockValue));
			}

			// بستن Flyout
			MinimumStockFlyout.IsOpen = false;
		}

	}
}
