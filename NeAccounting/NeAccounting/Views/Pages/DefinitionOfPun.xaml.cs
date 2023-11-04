using DomainShared.ViewModels;
using NeAccounting.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DefinitionOfPun.xaml
    /// </summary>
    public partial class DefinitionOfPun : INavigableView<DefinitoinOfPunViewModel>
    {
        public DefinitoinOfPunViewModel ViewModel { get; }
        public int materailId { get; set; }


        public DefinitionOfPun(DefinitoinOfPunViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void AutoSuggestBoxSuggestions_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {


            materailId = ((SuggestBoxViewModel<int>)args.SelectedItem).Id;
        }
    }
}
