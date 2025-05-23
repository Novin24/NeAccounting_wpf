﻿namespace DomainShared.ViewModels.Pun
{
	public class PunListDto
    {
        public Guid Id { get; set; }
        public string MaterialName { get; set; }
        public string UnitName { get; set; }
        public Guid UnitId { get; set; }
        public bool IsManufacturedGoods { get; set; }
        public double Entity { get; set; }
        public string SEntity { get; set; }
        public long LastSellPrice { get; set; }
        public long LastBuyPrice { get; set; }
        public string Serial { get; set; }
        public string Address { get; set; }
        public double MiniEntity { get; set; }
        public string ServiceType
        {
            get
            {
                return IsServise ? "خدماتی" : "کالا";
            }
        }
        public bool IsServise { get; set; }
        public bool IsActive { get; set; }

	}
}
