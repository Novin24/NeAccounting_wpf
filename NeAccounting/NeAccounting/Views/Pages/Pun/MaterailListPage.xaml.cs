using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages.Pun
{
    /// <summary>
    /// صفحه نمایش اجناس
    /// </summary>
    public partial class MaterailListPage :  INavigableView<MaterailListViewModel>
    {
        public MaterailListViewModel ViewModel { get; }

        public MaterailListPage(MaterailListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
