using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
    public class InvoiceDto
    {
        public long Price { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public long Serial { get; set; }
        public DocumntType Type { get; set; }
        public Guid Id { get; set; }
        public bool ReceivedOrPaid { get; set; }
    }
}
