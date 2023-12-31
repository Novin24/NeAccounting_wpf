using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateFinancialAidPage.xaml
    /// </summary>
    public partial class UpdateFinancialAidPage : INavigableView<CreateFinancialAidViewModel>
    {
        public CreateFinancialAidViewModel ViewModel { get; }

        public UpdateFinancialAidPage()
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
