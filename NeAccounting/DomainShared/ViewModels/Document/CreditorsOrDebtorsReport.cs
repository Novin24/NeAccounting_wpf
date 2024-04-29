namespace DomainShared.ViewModels.Document
{
    public class CreditorsOrDebtorsReport
    {
        public string? Name { get; set; }
        public long Debt { get; set; }
        public string Total { get; set; }
        public long Credit { get; set; }
        public string? ShamsiDate { get; set; }
    }
}
