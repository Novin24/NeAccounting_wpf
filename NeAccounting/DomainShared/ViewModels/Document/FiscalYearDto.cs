namespace DomainShared.ViewModels.Document
{
    public class FiscalYearDto
    {
        public Guid Id { get; set; }
        public int Row { get; set; }
        public string Titele { get; set; }
        public DateTime StartDate { get; set; }
        public string SStartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SEndDate { get; set; }
        public bool IsCurrent { get; set; }
        public bool NotActive { get; set; }
        public string Des { get; set; }
    }

}
