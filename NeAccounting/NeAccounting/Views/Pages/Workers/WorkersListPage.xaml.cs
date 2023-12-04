using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// صفحه نمایش کارگران
    /// </summary>
    public partial class WorkersListPage : INavigableView<WorkerListViewModel>
    {
        public WorkerListViewModel ViewModel { get; }
        public WorkersListPage(WorkerListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
