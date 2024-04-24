using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DebtorsListPage.xaml
    /// </summary>
    public partial class DebtorsListPage : INavigableView<DebtorsViewModel>
    {
        public DebtorsViewModel ViewModel { get; }

        public DebtorsListPage(DebtorsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        [RelayCommand]
        private async Task OnCreateCustomer()
        {
            Btn_submit.Focus();
            await ViewModel.SearchCommand.ExecuteAsync(null);
        }
    }
}
