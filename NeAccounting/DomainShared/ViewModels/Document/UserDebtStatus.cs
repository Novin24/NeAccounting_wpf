namespace DomainShared.ViewModels.Document
{
    public class UserDebtStatus
    {
        public required string Status { get; set; }
        public required string Amount { get; set; }
        public required long LAmount { get; set; }
    }
}
