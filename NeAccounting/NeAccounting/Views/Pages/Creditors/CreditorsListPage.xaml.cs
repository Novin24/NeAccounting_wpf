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
			Dtp_Start.Focus();

		}
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			Dtp_Start.nm_box.Focus(); 
		}
		[RelayCommand]
        private async Task OnSearch()
        {
            Btn_submit.Focus();
            await ViewModel.SearchCommand.ExecuteAsync(null);
        }


    }
}
