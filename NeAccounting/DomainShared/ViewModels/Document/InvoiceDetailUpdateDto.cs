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
        public DocumntType Type { get; set; }
        public string Serial { get; set; }
        public long TotalPrice { get; set; }
    }

    public class DocUpdateDto
    {
        public string? DocDescription { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime Date { get; set; }
        public long? Dicount { get; set; }
        public long Price { get; set; }
        public PaymentType Type { get; set; }
        public string Serial { get; set; }
    }
}
