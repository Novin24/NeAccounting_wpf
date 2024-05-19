namespace DomainShared.ViewModels.Document
{
    public class PunProfitListDto
    {
        public Guid MatId { get; set; }
        public double Count { get; set; }
        public long Total { get; set; }
        public long Fixedprice
        {
            get
            {
                return (long)Math.Round(Total / Count);
            }
        }
    }
}
