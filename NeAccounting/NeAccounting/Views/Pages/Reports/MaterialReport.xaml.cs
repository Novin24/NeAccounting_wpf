using DomainShared.ViewModels;
using NeAccounting.ViewModels;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for MaterialReport.xaml
    /// </summary>
    public partial class MaterialReportPage : INavigableView<MaterialReportViewModel>
    {
        public MaterialReportViewModel ViewModel { get; }
        public MaterialReportPage()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_name.Focus();
        }

        private void Pagination_PageChosen(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            if (!IsInitialized)
            {
                return;
            }
            ViewModel.ChangePageCommand.ExecuteAsync(null);
        }

        private void txt_name_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (!IsInitialized)
            {
                return;
            }
            var us = ((SuggestBoxViewModel<Guid, long>)args.SelectedItem);
            ViewModel.CusId = us.Id;
            ViewModel.PersonelId = us.UniqNumber;
        }
    }
}
