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
        private readonly ISnackbarService _snackbarService;
        public UnitViewModel ViewModel { get; }
        public UnitsListPage(UnitViewModel viewModel, ISnackbarService snackbarService)
        {
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
        private void CheckBox_Status_Unckecked(object sender, RoutedEventArgs e)
        {
            if (sender is not System.Windows.Controls.CheckBox btn)
                return;

            if (btn.Tag == null)
                return;

            int id = int.Parse(btn.Tag.ToString());
            var unit = ViewModel.List.First(x => x.Id == id);
            if (!unit.IsActive)
            {
                return;
            }
            ViewModel.DeActiveCommand.ExecuteAsync(id);
        }
    }
}
