using Domain.Common;
using Domain.Enities.NovinEntity.Remittances;

namespace Domain.NovinEntity.Materials
{
    public class Material : LocalEntity
    {
        #region Navigation
        public Unit Unit { get; set; }
        public int UnitId { get; set; }
        public IEnumerable<SellRemittance> SellRemittances { get; set; }
        public IEnumerable<BuyRemittance> BuyRemittances { get; set; }
        #endregion

        #region Property
        public string Name { get; set; }
        public string Serial { get; set; }
        public double Entity { get; set; }
        public long LastSellPrice { get; set; }
        public long LastBuyPrice { get; set; }
        public bool IsManufacturedGoods { get; set; }
        public string PhysicalAddress { get; set; }
        public bool IsActive { get; set; }
        public bool IsService { get; set; }
        #endregion

        #region Constructor
        internal Material()
        {

        }

        public Material(string name,
            int unitId,
            bool isService,
            long lastSellPrice,
            string serial,
            string physicalAddress,
            bool isManufacturedGoods)
        {
            Name = name;
            IsManufacturedGoods = isManufacturedGoods;
            UnitId = unitId;
            Serial = serial;
            Entity = 0;
            LastBuyPrice = 0;
            LastSellPrice = lastSellPrice;
            PhysicalAddress = physicalAddress;
            IsActive = true;
        }
        #endregion
    }
}
