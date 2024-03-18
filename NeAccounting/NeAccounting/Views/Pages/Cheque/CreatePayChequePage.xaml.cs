using Wpf.Ui.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreatePayCheque.xaml
    /// </summary>
    public partial class CreatePayChequePage : INavigableView<CreatePayChequeViewModel>
    {
        public CreatePayChequeViewModel ViewModel { get; }

        public CreatePayChequePage(CreatePayChequeViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txt_Name.Focus();
        }
    }
}
