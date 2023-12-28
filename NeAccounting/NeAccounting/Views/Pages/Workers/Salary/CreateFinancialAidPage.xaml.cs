using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateFinancialAidPage.xaml
    /// </summary>
    public partial class CreateFinancialAidPage : INavigableView<CreateFinancialAidViewModel>
    {
        public CreateFinancialAidViewModel ViewModel { get; }
        public CreateFinancialAidPage(CreateFinancialAidViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ViewModel.WorkerId = ((SuggestBoxViewModel<int>)args.SelectedItem).Id;
        }
    }
}
