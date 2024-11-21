using Domain.Common;
using Domain.Enities.NovinEntity.Remittances;
using DomainShared.Errore;

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
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// سریال
        /// </summary>
        public string Serial { get; private set; }
        public double Entity { get; set; }
        public long LastSellPrice { get; set; }
        public long LastBuyPrice { get; set; }
        public bool IsManufacturedGoods { get; set; }
        public string PhysicalAddress { get; private set; }
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
            SetName (name);
            SetSerial(serial);
            SetAddress(physicalAddress);
            IsManufacturedGoods = isManufacturedGoods;
            UnitId = unitId;
            IsService = isService;
            Entity = 0;
            LastBuyPrice = 0;
            LastSellPrice = lastSellPrice;
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

        public Pun(
            Guid id,
            string name,
            Guid unitId,
            bool isService,
            long lastSellPrice,
            string serial,
            double entity,
            long lastBuyPrice,
            bool isActive,
            string physicalAddress)
        {
            Id = id;
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

        #region Methods
        public Pun SetName(string name)
        {
            if (name.Length > 100)
                throw new ArgumentException(NeErrorCodes.IsLess("عنوان", "صد"));

            Name = name;
            return this;
        }

        public Pun SetSerial(string serial)
        {
            if (!string.IsNullOrEmpty(serial) && serial.Length > 50)
                throw new ArgumentException(NeErrorCodes.IsLess("سریال", "پنجاه"));

            Serial = serial;
            return this;
        }

        public Pun SetAddress(string physicalAddress)
        {
            if (!string.IsNullOrEmpty(physicalAddress) && physicalAddress.Length > 100)
                throw new ArgumentException(NeErrorCodes.IsLess("ادرس فیزیکی", "صد"));

            PhysicalAddress = physicalAddress;
            return this;
        }
        #endregion
    }
}
