using DomainShared.Enums;
using NeAccounting.ViewModels;
using Wpf.Ui.Controls;
using DomainShared.Utilities;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateExpencePage.xaml
    /// </summary>
    public partial class CreateExpencePage : INavigableView<CreateExpenceViewModel>
    {
        public CreateExpenceViewModel ViewModel { get; }

        public CreateExpencePage(CreateExpenceViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            DateFocus.Focus();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Cmb_Status.ItemsSource = PaymentType.Cash.ToEnumDictionary();
        }
    }
}
