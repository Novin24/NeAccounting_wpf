using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateFunctionPage.xaml
    /// </summary>
    public partial class UpdateFunctionPage : INavigableView<UpdateFunctionViewModel>
    {
        public UpdateFunctionViewModel ViewModel { get; }
        public UpdateFunctionPage(UpdateFunctionViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            txt_sugName.Focus();
        }
    }
}
