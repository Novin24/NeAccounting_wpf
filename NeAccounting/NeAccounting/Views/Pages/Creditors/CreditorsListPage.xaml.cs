using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreditorsListPage.xaml
    /// </summary>
    public partial class CreditorsListPage : INavigableView<CreditorsViewModel>
    {
        public CreditorsViewModel ViewModel { get; }

        public CreditorsListPage(CreditorsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();

        }
        [RelayCommand]
        private async Task OnSearch()
        {
            Btn_submit.Focus();
            await ViewModel.SearchCommand.ExecuteAsync(null);
        }


    }
}
