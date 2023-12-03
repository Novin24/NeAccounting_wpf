using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// صفحه به روز رسانی کارگران
    /// </summary>
    public partial class UpdateWorkerPage : INavigableView<UpdateWorkerViewModel>
    {
        public UpdateWorkerViewModel ViewModel { get; }
        public UpdateWorkerPage(UpdateWorkerViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
