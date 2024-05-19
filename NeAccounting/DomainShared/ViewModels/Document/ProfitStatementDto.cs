namespace DomainShared.ViewModels.Document
{
    public class ProfitStatementDto
    {
        public long ProfitLossStatement { get; set; }
        public long Inventory { get; set; }
        public long TotalSell { get; set; }
        public long TotalBuy { get; set; }
    }
}
