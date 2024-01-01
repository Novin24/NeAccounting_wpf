using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateFunctionPage.xaml
    /// </summary>
    public partial class CreateFunctionPage : INavigableView<CreateFunctionViewModel>
    {
        public CreateFunctionViewModel ViewModel { get; }

        public CreateFunctionPage(CreateFunctionViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            ViewModel.WorkerId = ((PersonnerlSuggestBoxViewModel)args.SelectedItem).Id;
            ViewModel.PersonelId = ((PersonnerlSuggestBoxViewModel)args.SelectedItem).PersonnelId;
        }
    }
}
