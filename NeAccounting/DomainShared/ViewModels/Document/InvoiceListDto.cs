using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
    public class InvoiceListDto
    {
        public int Row { get; set; }
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string? ShamsiDate { get; set; }
        public string? Description { get; set; }
        public DocumntType Type { get; set; }
        public long Bes { get; set; }
        public long Bed { get; set; }
        public long LeftOver { get; set; }
        public string? Status { get; set; }
        public bool IsDeletable { get; set; }
        public bool IsEditable { get; set; }
    }

    public class InvoiceDto
    {
        public long Price { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Serial { get; set; }
        public DocumntType Type { get; set; }
        public Guid Id { get; set; }
        public bool ReceivedOrPaid { get; set; }
    }
}
