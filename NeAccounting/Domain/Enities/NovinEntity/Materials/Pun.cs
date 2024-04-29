using Domain.Common;
using Domain.Enities.NovinEntity.Remittances;

namespace Domain.NovinEntity.Materials
{
    public class Pun : LocalEntity<Guid>
    {
        #region Navigation
        public Units Unit { get; set; }
        public Guid UnitId { get; set; }
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
        internal Pun()
        {

        }

        public Pun(string name,
            Guid unitId,
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
            IsService = isService;
            Entity = 0;
            LastBuyPrice = 0;
            LastSellPrice = lastSellPrice;
            PhysicalAddress = physicalAddress;
            IsActive = true;
        }

        public Pun(string name,
            Guid unitId,
            bool isService,
            long lastSellPrice,
            string serial,
            double entity,
            long lastBuyPrice,
            bool isActive,
            string physicalAddress)
        {
            Name = name;
            IsManufacturedGoods = false;
            Entity = entity;
            LastBuyPrice = lastBuyPrice;
            UnitId = unitId;
            Serial = serial;
            IsService = isService;
            LastSellPrice = lastSellPrice;
            PhysicalAddress = physicalAddress;
            IsActive = isActive;
        }
        #endregion

    }
}
