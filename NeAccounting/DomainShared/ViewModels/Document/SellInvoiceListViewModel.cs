namespace DomainShared.ViewModels.Document
{
    public class RemittanceListViewModel
    {
        public int RowId { get; set; }
        public Guid RremId { get; set; }
        public string? MatName { get; set; }
        public string? UnitName { get; set; }
        public long Price { get; set; }
        public bool IsService { get; set; }
        public int MaterialId { get; set; }
        public double AmountOf { get; set; }
        public long TotalPrice { get; set; }
        public string? Description { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
