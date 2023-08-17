using Np_Accounting.ViewModels;
using Wpf.Ui.Controls;

namespace Np_Accounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for payment.xaml
    /// </summary>
    public partial class PaymentPage : INavigableView<PaymentViewModel>
    {
        public PaymentViewModel ViewModel
        {
            get;
        }

        public PaymentPage(PaymentViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
