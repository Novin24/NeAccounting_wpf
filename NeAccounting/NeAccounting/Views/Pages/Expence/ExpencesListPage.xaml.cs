using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for ExpensesListPage.xaml
    /// </summary>
    public partial class ExpencesListPage : INavigableView<ExpencelistViewModel>
    {
        public ExpencelistViewModel ViewModel { get; }
        public ExpencesListPage(ExpencelistViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            date.Focus();
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
