namespace DomainShared.ViewModels.Pun
{
    public struct MatListDto
    {
        public Guid Id { get; set; }
        public string MaterialName { get; set; }
        public string UnitName { get; set; }
        public double Entity { get; set; }
        public bool IsManufacturedGoods { get; set; }
        public bool IsService { get; set; }
        public long LastSellPrice { get; set; }
        public long LastBuyPrice { get; set; }
        public List<RawMaterial> RawMaterials { get; set; }
    }

    public struct RawMaterial
    {
        /// <summary>
        /// درصد ضایعات
        /// </summary>
        public byte WastePercentage { get; set; }
        /// <summary>
        /// درصد استفاده
        /// </summary>
        public byte UsagePercentage { get; set; }

        /// <summary>
        /// نسبت کالای تولیدی به مواد اول
        /// مثال :> تولید هر متر لوله برابر است با پنج کیلو گرانول و صد گرم رنگدانه و یک متر نوار چاپ
        /// </summary>
        public double Ratio { get; set; }
        public string MaterialName { get; set; }
        public Guid MaterialId { get; set; }
        public string UnitName { get; set; }
    }
}
