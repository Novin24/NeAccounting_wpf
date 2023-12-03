using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// صفحه نمایش کارگران
    /// </summary>
    public partial class WorkersList : INavigableView<CreateWorkerViewModel>
    {
        public CreateWorkerViewModel ViewModel { get; }
        public WorkersList(CreateWorkerViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
