using DomainShared.ViewModels.Invoice;
using DomainShared.ViewModels.Pun;
using System.Windows.Controls;

namespace NeAccounting.Pages
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
