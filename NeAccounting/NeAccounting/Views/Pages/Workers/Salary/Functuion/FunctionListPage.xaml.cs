using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for FunctionListPage.xaml
    /// </summary>
    public partial class FunctionListPage : INavigableView<FunctionListViewModel>
    {
        public FunctionListViewModel ViewModel { get; }

        public FunctionListPage(FunctionListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ViewModel.WorkerId = ((PersonnerlSuggestBoxViewModel)args.SelectedItem).Id;
        }
    }
}
