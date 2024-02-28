using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UnitsListPage.xaml
    /// </summary>
    public partial class UnitsListPage : INavigableView<UnitViewModel>
    {
        public UnitViewModel ViewModel { get; }
        public UnitsListPage(UnitViewModel viewModel)
        {
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

            int id = int.Parse(btn.Tag.ToString());
            var unit = ViewModel.List.First(x => x.Id == id);
            txt_name.Text = unit.UnitName;
            txt_description.Text = unit.Description;
        }
    }
}
