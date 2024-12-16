using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for FinancialAidListPage.xaml
    /// </summary>
    public partial class FinancialAidListPage : INavigableView<AidListViewModel>
    {
        public AidListViewModel ViewModel { get; }
        public FinancialAidListPage(AidListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ViewModel.WorkerId = ((PersonnerlSuggestBoxViewModel)args.SelectedItem).Id;
		}
		private void Pagination_PageChosen(object sender, RoutedPropertyChangedEventArgs<int> e)
		{
			if (!IsInitialized)
			{
				return;
			}
			ViewModel.PageChengeCommand.Execute(null);
		}
	}
}
