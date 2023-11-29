using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages.Workers
{
    /// <summary>
    /// صفحه نمایش کارگران
    /// </summary>
    public partial class Workers : INavigableView<WorkerListViewModel>
    {
        public WorkerListViewModel ViewModel { get; }

        public Workers(WorkerListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
