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
        public virtual ICollection<PunProduct> Productions { get; set; } = [];
        /// <summary>
        /// مواد خامی که این جنس  برای تولید لازم دارد
        /// </summary>
        public virtual ICollection<PunProduct> RawMaterials { get; set; } = [];
        #endregion

        #region Property
        public string Name { get; private set; }
        public string Serial { get; private set; }
        public double Entity { get; set; }
        public long LastSellPrice { get; set; }
        public long LastBuyPrice { get; set; }
        public bool IsManufacturedGoods { get; set; }
        public bool IsRawMaterial { get; set; }
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
            bool isManufacturedGoods,
            bool isRawmaterial,
            List<PunProduct> pnp)
        {
            SetName(name);
            SetSerial(serial);
            SetAddress(physicalAddress);
            SetRawMaterials(pnp);
            IsManufacturedGoods = isManufacturedGoods;
            IsRawMaterial = isRawmaterial;
            UnitId = unitId;
            IsService = isService;
            Entity = 0;
            LastBuyPrice = 0;
            LastSellPrice = lastSellPrice;
            IsActive = true;
        }


        /// <summary>
        /// برای افزودن اجناس یکجا در سال مالی جدید
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="unitId"></param>
        /// <param name="isService"></param>
        /// <param name="lastSellPrice"></param>
        /// <param name="serial"></param>
        /// <param name="entity"></param>
        /// <param name="lastBuyPrice"></param>
        /// <param name="isActive"></param>
        /// <param name="physicalAddress"></param>
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

        public Pun SetRawMaterials(List<PunProduct> rawMaterials)
        {
            if (IsManufacturedGoods && rawMaterials.Count == 0)
                throw new ArgumentException(NeErrorCodes.IsMandatory("مواد اولیه"));

            RawMaterials = rawMaterials;
            return this;
        }
        #endregion
    }
}
