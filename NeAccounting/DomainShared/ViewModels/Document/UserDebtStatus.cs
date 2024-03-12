namespace DomainShared.ViewModels.Document
{
    public class UserDebtStatus
    {
        public required string Status { get; set; }
        public long Amount { get; set; }
        public required string Credit { get; set; }
        public required string Debt { get; set; }
    }
}
