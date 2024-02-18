namespace DomainShared.ViewModels.Document
{
    public class DetailRemittanceDto
    {
        public int Row { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string ShamsiDate { get; set; }
        public long Bes { get; set; }
        public long Price { get; set; }
        public long Bed { get; set; }
        public long LeftOver { get; set; }
        public string Status { get; set; }
        public string Unit { get; set; }
        public string Story { get; set; }
        public string Cus_Name { get; set; }
        public Guid Cus_ID { get; set; }
        public double AmuontOf { get; set; }
    }
}
