using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for FiscalYearListPage.xaml
    /// </summary>
    public partial class FiscalYearListPage :  INavigableView<FiscalYearViewModel>
    {
        public FiscalYearViewModel ViewModel { get; }

    public FiscalYearListPage(FiscalYearViewModel viewModel)
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
    }
}
