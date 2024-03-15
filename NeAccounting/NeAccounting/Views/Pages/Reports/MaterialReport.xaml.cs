using DomainShared.ViewModels;
using DomainShared.ViewModels.Pun;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for MaterialReport.xaml
    /// </summary>
    public partial class MaterialReportPage : INavigableView<MaterialReportViewModel>
    {
        public MaterialReportViewModel ViewModel { get; }
        public MaterialReportPage(MaterialReportViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
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
            var us = ((PunListDto)args.SelectedItem);
            ViewModel.MaterialId = us.Id;
        }

        private void root_Loaded(object sender, RoutedEventArgs e)
        {
            txt_name.Focus();
        }

    }
}
