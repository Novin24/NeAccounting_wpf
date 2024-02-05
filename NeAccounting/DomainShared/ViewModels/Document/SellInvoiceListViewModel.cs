namespace DomainShared.ViewModels.Document
{
    public class RemittanceListViewModel
    {
        public int RremId { get; set; }
        public int MaterialId { get; set; }
        public double AmountOf { get; set; }
        public uint TotalPrice { get; set; }
        public string? Description { get; set; }
    }
}
