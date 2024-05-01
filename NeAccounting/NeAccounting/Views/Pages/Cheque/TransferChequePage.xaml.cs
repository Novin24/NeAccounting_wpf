using DomainShared.Enums;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for TransferChequePage.xaml
    /// </summary>
    public partial class TransferChequePage : INavigableView<TransferChequeViewModel>
    {
        public TransferChequeViewModel ViewModel { get; }

        public TransferChequePage(TransferChequeViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_Name.Focus();
            Cmb_Status.ItemsSource = SubmitChequeStatus.Register.ToEnumDictionary();
        }

        private void Txt_Name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var user = (SuggestBoxViewModel<Guid, long>)args.SelectedItem;
            if (DataContext is TransferChequePage c)
            {
                var ts = c.ViewModel;
                ts.CusId = user.Id;
            }
            lbl_cusId.Text = user.UniqNumber.ToString();
        }

        [RelayCommand]
        private async Task OnSubmit()
        {
            Btn_submit.Focus();
            await ViewModel.SubmitCommand.ExecuteAsync(null);
        }
    }
}
