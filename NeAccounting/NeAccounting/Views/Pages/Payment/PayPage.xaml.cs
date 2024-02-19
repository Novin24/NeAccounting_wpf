using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Pages
{
    /// <summary>
    /// Interaction logic for PayPage.xaml
    /// </summary>
    public partial class PayPage : INavigableView<CreatePayDocViewModel>
    {
        public CreatePayDocViewModel ViewModel { get; }
        public Guid CusId { get; set; }

        public PayPage(CreatePayDocViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private async void AutoSuggestBoxSuggestions_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
                return;
            var user = (SuggestBoxViewModel<Guid, long>)args.SelectedItem;
            ViewModel.CusId = user.Id;
            lbl_cusId.Text = user.UniqNumber.ToString();
            await ViewModel.OnSelectCus(user.Id);
        }
    }
}
