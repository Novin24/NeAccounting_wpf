using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// صفحه ایجاد کارگران
    /// </summary>
    public partial class CreateWorkerPage : INavigableView<CreateWorkerViewModel>
    {
        public CreateWorkerViewModel ViewModel { get; }
        public CreateWorkerPage(CreateWorkerViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
