using DomainShared.ViewModels;
using Microsoft.VisualBasic.ApplicationServices;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
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
            if (DataContext is UpdateChequePage c)
            {
                var ts = c.ViewModel;
                ts.CusId = user.Id;
            }
            lbl_cusId.Text = user.UniqNumber.ToString();
        }

        [RelayCommand]
        private async Task OnCreateCustomer()
        {
            if (DataContext is UpdateChequePage c)
            {
            Btn_submit.Focus();
            await c.ViewModel.SubmitCommand.ExecuteAsync(null);

            }
        }
    }
}
