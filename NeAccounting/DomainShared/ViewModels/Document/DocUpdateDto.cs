using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
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
