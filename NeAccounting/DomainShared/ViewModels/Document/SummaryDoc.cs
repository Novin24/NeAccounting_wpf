namespace DomainShared.ViewModels.Document
{
    public class SummaryDoc
    {
        public DateTime SubmitDate { get; set; }
        public string? ShamsiDate { get; set; }
        public string? Price { get; set; }
        public string? Cus_Name { get; set; }
    }

    public class CreditorsOrDebtorsReport
    {
        public string? CusName { get; set; }
        public string? Price { get; set; }
        public DateTime Date { get; set; }
        public string? ShamsiDate { get; set; }
    }
}
