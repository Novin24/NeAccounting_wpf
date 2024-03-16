using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
    public class InvoiceDetailUpdateDto
    {
        public List<RemittanceListViewModel> RemList { get; set; } = [];
        public string? InvoiceDescription { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public double? Commission { get; set; }
        public long? CommissionPrice { get; set; }
        public string CommissionSPrice { get { return CommissionPrice.HasValue ? CommissionPrice.Value.ToString("N0") : "0"; } }
        public DocumntType Type { get; set; }
        public string Serial { get; set; }
        public long TotalPrice { get; set; }
    }
}
