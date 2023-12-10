using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Pages
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class RecPage : INavigableView<RecViewModel>
    {
        public RecViewModel ViewModel { get; }

        public RecPage(RecViewModel viewModel)
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

        }
    }
}
