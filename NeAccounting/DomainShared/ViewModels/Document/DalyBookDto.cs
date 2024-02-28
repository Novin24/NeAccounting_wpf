using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
    public class DalyBookDto
    {
        public int Row { get; set; }
        public string? ShamsiDate { get; set; }
        public string? CustomerName { get; set; }
        public string? Description { get; set; }
        public string? Serial { get; set; }
        public string? Bed { get; set; }
        public string? Bes { get; set; }
        public Guid Id { get; set; }
        public DocumntType Type { get; set; }
        public DateTime SubmitDate { get; set; }
    }
}
