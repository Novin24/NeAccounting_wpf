using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for DailyBook.xaml
    /// </summary>
    public partial class DailyBookPage : INavigableView<DalyBookViewModel>
    {
        public DalyBookViewModel ViewModel { get; }
        public DailyBookPage(DalyBookViewModel viewModel)
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
            ViewModel.PageChengeCommand.Execute(null);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Dtp_date.txt_date.Focus();
        }
    }
}
