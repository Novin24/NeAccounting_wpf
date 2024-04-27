using NeAccounting.ViewModels;
using System.Windows.Media;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UnitsListPage.xaml
    /// </summary>
    public partial class UnitsListPage : INavigableView<UnitViewModel>
    {
        private readonly IContentDialogService _contentDialogService;
        private readonly ISnackbarService _snackbarService;
        public UnitViewModel ViewModel { get; }
        public UnitsListPage(IContentDialogService contentDialogService, UnitViewModel viewModel, ISnackbarService snackbarService)
        {
            _contentDialogService = contentDialogService;
            _snackbarService = snackbarService;
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_name.Focus();
        }

        private void btn_Update_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn)
                return;

            if (btn.Tag == null)
                return;

            if (ViewModel.UnitId != null)
            {
                _snackbarService.Show("کاربر گرامی", "لطفا ابتدا فیلد ویرایشی را ثبت سپس اقدام به ویرایش مجدد نمایید !!", ControlAppearance.Secondary, new SymbolIcon(SymbolRegular.Warning20, new SolidColorBrush(Colors.Goldenrod)), TimeSpan.FromMilliseconds(3000));
                return;
            }

            int id = int.Parse(btn.Tag.ToString());
            var unit = ViewModel.List.First(x => x.Id == id);
            ViewModel.UnitName = unit.UnitName;
            ViewModel.Description = unit.Description;
            ViewModel.UnitId = unit.Id;
            ViewModel.List.Remove(unit);
            dgv_Inv.Items.Refresh();
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
                Title = "آیا از بایگانی اطمینان دارید؟",
                Content = new TextBlock() { Text = "پس از بایگانی واحد مد نظر در عملیات ثبت و ویرایش (اجناس و خدمات) نمایش داده نمیشود!!!", FlowDirection = FlowDirection.RightToLeft, FontFamily = new FontFamily("Calibri"), FontSize = 16 },
                PrimaryButtonText = "بله",
                SecondaryButtonText = "خیر",
                CloseButtonText = "انصراف",
            });
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.DeActiveCommand.ExecuteAsync(id);
            }
        }

        [RelayCommand]
        private async Task OnCreateUnit()
        {
            Btn_submit.Focus();
            await ViewModel.CreateUnitCommand.ExecuteAsync(null);
        }
    }
}
