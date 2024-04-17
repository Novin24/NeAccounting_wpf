using DomainShared.Enums;
using DomainShared.Utilities;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for NotificationListPage.xaml
    /// </summary>
    public partial class NotificationListPage : INavigableView<NotifListViewModel>
    {
        public NotifListViewModel ViewModel { get; }

        public NotificationListPage(NotifListViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Txt_Titele.Focus();
            cmb_Priority.ItemsSource = Priority.All.ToEnumDictionary();
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
