namespace DomainShared.ViewModels.Pun
{
    public struct PunListDto
    {
        public int Id { get; set; }
        public string MaterialName { get; set; }
        public string UnitName { get; set; }
        public int UnitId { get; set; }
        public bool IsManufacturedGoods { get; set; }
        public double Entity { get; set; }
        public long LastSellPrice { get; set; }
        public string Serial { get; set; }
        public string Address { get; set; }
    }
}
