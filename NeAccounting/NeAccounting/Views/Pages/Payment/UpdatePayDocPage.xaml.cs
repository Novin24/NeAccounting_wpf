using DomainShared.Enums;
using DomainShared.Utilities;
using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for UpdatePayPage.xaml
    /// </summary>
    public partial class UpdatePayDocPage : INavigableView<UpdatePayDocViewModel>
    {
        public UpdatePayDocViewModel ViewModel { get; set; }
        public UpdatePayDocPage(UpdatePayDocViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Cmb_PayType.ItemsSource = PaymentType.CardToCard.ToDictionary();
        }
    }
}
