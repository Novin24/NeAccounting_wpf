using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{

    /// <summary>
    /// Interaction logic for SalaryListPage.xaml
    /// </summary>
    public partial class SalaryListPage : INavigableView<SalaryListViewModel>
    {
        public SalaryListViewModel ViewModel { get; }

        public SalaryListPage(SalaryListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void autoSuggest_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ViewModel.WorkerId = ((PersonnerlSuggestBoxViewModel)args.SelectedItem).Id;
        }
    }
}
