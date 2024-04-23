namespace DomainShared.ViewModels.Document
{
    public class DetailRemittanceDto
    {
        public int Row { get; set; }
        public bool IsLeftOver { get; set; } = false;
        public DateTime Date { get; set; }
        public string? ShamsiDate { get; set; }
        public string? Description { get; set; }
        public long Bes { get; set; }
        public long Bed { get; set; }
        public long LeftOver { get; set; }
        public string? Status { get; set; }
        public string? Serial { get; set; }
        public string? Price { get; set; }
        public string? Unit { get; set; }
        public string? AmuontOf { get; set; }
        public string? MaterialName { get; set; }
        public bool IsRecived { get; set; }
    }
}
