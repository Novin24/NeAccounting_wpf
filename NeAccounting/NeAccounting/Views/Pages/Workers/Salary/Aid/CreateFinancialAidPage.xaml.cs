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
            ViewModel.WorkerId = ((PersonnerlSuggestBoxViewModel)args.SelectedItem).Id;
            ViewModel.PersonelId = ((PersonnerlSuggestBoxViewModel)args.SelectedItem).PersonnelId;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_sugName.Focus();
        }
        [RelayCommand]
        private async Task OnCreate()
        {
            Btn_submit.Focus();
            await ViewModel.CreateCommand.ExecuteAsync(null);
        }

        private void txt_CustomerName_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = ViewModel.Loding;
        }
    }
}
