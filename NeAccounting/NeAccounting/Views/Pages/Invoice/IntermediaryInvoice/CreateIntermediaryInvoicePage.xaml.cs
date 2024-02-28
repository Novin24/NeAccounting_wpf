using DomainShared.ViewModels.Invoice;
using System.Windows.Controls;

namespace NeAccounting.Views.Pages
{
    /// <summary>
    /// Interaction logic for CreateIntermediaryInvoicePage.xaml
    /// </summary>
    public partial class CreateIntermediaryInvoicePage : Page
    {
        private IEnumerable<testinviceDto> list;
        public CreateIntermediaryInvoicePage()
        {
            InitializeComponent();
        }

    }
}
