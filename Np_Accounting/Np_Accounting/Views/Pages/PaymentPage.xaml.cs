using Np_Accounting.ViewModels;
using Wpf.Ui.Common.Interfaces;

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
            this.ViewModel = viewModel;
            InitializeComponent();
        }
    }
}
