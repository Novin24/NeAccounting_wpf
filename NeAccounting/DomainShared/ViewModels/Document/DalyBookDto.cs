using DomainShared.Enums;

namespace DomainShared.ViewModels.Document
{
    public class DalyBookDto
    {
        public int Row { get; set; }
        public string? ShamsiDate { get; set; }
        public string? CustomerName { get; set; }
        public string? Description { get; set; }
        public long Bed { get; set; }
        public long Bes { get; set; }
        public Guid Id { get; set; }
        public DocumntType Type{ get; set; }
    }
}
