using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages.Cheque
{
    /// <summary>
    /// Interaction logic for UpdateCheque.xaml
    /// </summary>
    public partial class UpdateChequePage : INavigableView<UpdateChequeViewModel>
    {

        public UpdateChequeViewModel ViewModel { get; }

        public UpdateChequePage(UpdateChequeViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is UpdateChequePage c)
            {
                var ts = c.ViewModel;
                txt_Name.SetCurrentValue(AutoSuggestBox.TextProperty, ts.CusName);
            }
            //Cmb_Status.ItemsSource = SubmitChequeStatus.Register.ToEnumDictionary();
        }

        private void Txt_Name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var user = (SuggestBoxViewModel<Guid, long>)args.SelectedItem;
            if (DataContext is UpdateChequePage c)
            {
                var ts = c.ViewModel;
                ts.CusId = user.Id;
            }
            lbl_cusId.Text = user.UniqNumber.ToString();
        }
    }
}
