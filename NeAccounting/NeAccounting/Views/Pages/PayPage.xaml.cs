using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Pages
{
    /// <summary>
    /// Interaction logic for PayPage.xaml
    /// </summary>
    public partial class PayPage : INavigableView<PayViewModel>
    {
        public PayViewModel ViewModel { get; }
        public Guid cusId { get; set; }

        public PayPage(PayViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            Lastchecks One = new Lastchecks();
            One.LastchecksAmount = "286,000";
            One.LastchecksDate = "12/12/1402";
            LastChecksdata.Items.Add(One);

            Lastchecks two = new Lastchecks();
            two.LastchecksAmount = "286,000";
            two.LastchecksDate = "12/12/1402";
            LastChecksdata.Items.Add(two);
        }
        public class Lastchecks
        {
            public string LastchecksAmount { set; get; }
            public string LastchecksDate { set; get; }

        }

        private void AutoSuggestBoxSuggestions_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
          //var t =   ViewModel.AutoSuggestBoxSuggestions.FirstOrDefault(a => a.Id == 
          //((SuggestBoxViewModel<Guid>)args.SelectedItem).Id);

            cusId = ((SuggestBoxViewModel<Guid>)args.SelectedItem).Id;

        }
    }
}
