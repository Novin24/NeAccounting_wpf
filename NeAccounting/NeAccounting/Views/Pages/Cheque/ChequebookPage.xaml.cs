using DomainShared.Enums;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using System.Text.RegularExpressions;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for Chequebook.xaml
    /// </summary>
    public partial class ChequebookPage : INavigableView<ChequebookViewModel>
    {
        public ChequebookViewModel ViewModel { get; }

        public ChequebookPage(ChequebookViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_Name.Focus();
            Cmb_Status.ItemsSource = ChequeStatus.AllCheques.ToEnumDictionary();
        }

        private void Pagination_PageChosen(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (!IsInitialized)
            {
                return;
            }
            ViewModel.ChangePageCommand.ExecuteAsync(null);
        }

        private void Txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
            {
                return;
            }
            var us = ((SuggestBoxViewModel<Guid, long>)args.SelectedItem);
            ViewModel.CusId = us.Id;
            ViewModel.PersonelId = us.UniqNumber;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = MyRegex().IsMatch(e.Text);
        }

        [GeneratedRegex("[^0-9]+")]
        private static partial Regex MyRegex();
    }
}
