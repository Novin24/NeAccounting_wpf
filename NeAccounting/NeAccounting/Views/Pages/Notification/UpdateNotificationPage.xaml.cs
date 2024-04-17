using DomainShared.Enums;
using DomainShared.Utilities;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdateNotificationPage.xaml
    /// </summary>
    public partial class UpdateNotificationPage : INavigableView<UpdateNotifViewModel>
    {
        public UpdateNotifViewModel ViewModel { get; }

        public UpdateNotificationPage(UpdateNotifViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Txt_Titele.Focus();
            //var t = Priority.All.ToEnumDictionary();
            //t.Remove(Priority.All);
            //cmb_Priority.ItemsSource = t;
        }
    }
}
