namespace DomainShared.ViewModels.Pun
{
    public struct PunListDto
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public string UnitName { get; set; }
        public double Entity { get; set; }
        public long SellPrice { get; set; }
        public long BuyPrice { get; set; }
        public long LastPrice { get; set; }
        public string Serial { get; set; }
        public string Address { get; set; }
    }
}
