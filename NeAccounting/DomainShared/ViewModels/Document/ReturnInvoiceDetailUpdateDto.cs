namespace DomainShared.ViewModels.Document
{
    public class ReturnInvoiceDetailUpdateDto
    {
        public List<RemittanceListViewModel> ParentRemList { get; set; } = [];
        public List<RemittanceListViewModel> ReturnRemList { get; set; } = [];
        public Guid CustomerId { get; set; }
        public string ParentSerial { get; set; }
        public string? Description { get; set; }
        public long TotalInvPrice { get; set; }
        public DateTime Date { get; set; }
        public string ReturnSerial { get; set; }
    }
}
