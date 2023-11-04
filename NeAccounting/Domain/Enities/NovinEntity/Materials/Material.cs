using Domain.Common;

namespace Domain.NovinEntity.Materials
{
    public class Material : LocalEntity
    {
        #region Navigation
        public Unit Unit{ get;private set; }
        public int UnitId { get; private set; }
        #endregion

        #region Property
        public string Name { get; private set; }
        public string Serial { get; private set; }
        public double Entity { get; private set; }
        public long LastPrice { get; private set; }
        public long SellPrice { get; private set; }
        public long BuyPrice { get; private set; }
        public string PhysicalAddress { get; private set; }
        public bool Active { get; private set; }
        #endregion

        #region Constructor
        internal Material()
        {

        }

        public Material(string name,
            int unitId,
            string serial,
            double entity,
            long lastPrice,
            long sellPrice,
            long buyPrice,
            string physicalAddress)
        {
            Name = name;
            UnitId = unitId;
            Serial = serial;
            Entity = entity;
            LastPrice = lastPrice;
            SellPrice = sellPrice;
            BuyPrice = buyPrice;
            PhysicalAddress = physicalAddress;
            Active = true;
        }
        #endregion
    }
}
