using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages.Pun
{
    /// <summary>
    /// صفحه نمایش اجناس
    /// </summary>
    public partial class PunListPage :  INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public PunListPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
