namespace DomainShared.ViewModels.Pun
{
    public struct MatListDto
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public string UnitName { get; set; }
        public double Entity { get; set; }
        public long LastSellPrice { get; set; }
        public long LastBuyPrice { get; set; }
    }
}
