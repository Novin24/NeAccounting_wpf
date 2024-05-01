using DomainShared.Enums;
using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateTransferChequePage.xaml
    /// </summary>
    public partial class UpdateTransferChequePage : INavigableView<UpdateTransferChequeViewModel>
    {

        public UpdateTransferChequeViewModel ViewModel { get; }

        public UpdateTransferChequePage(UpdateTransferChequeViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is UpdateTransferChequePage c)
            {
                txt_Name.SetCurrentValue(AutoSuggestBox.TextProperty, c.ViewModel.CusName);
            }
            txt_Name.Focus();
            //Cmb_Status.ItemsSource = SubmitChequeStatus.Register.ToEnumDictionary();
        }

        private void Txt_Name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var user = (SuggestBoxViewModel<Guid, long>)args.SelectedItem;
            if (DataContext is UpdateTransferChequePage c)
            {
                var ts = c.ViewModel;
                ts.CusId = user.Id;
            }
            lbl_cusId.Text = user.UniqNumber.ToString();
        }
    }
}
