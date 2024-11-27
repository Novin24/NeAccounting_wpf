namespace DomainShared.ViewModels.Pun
{
	public class ImportMaterailListDto
	{
		/// <summary>
		/// نام اجناس
		/// </summary>
		public string MaterialName { get; set; }

		/// <summary>
		/// نام واحد
		/// </summary>
		public string UnitName { get; set; }

		/// <summary>
		/// شناسه واحد
		/// </summary>
		public Guid UnitId { get; set; }

		/// <summary>
		/// آخرین قیمت فروش
		/// </summary>
		public long LastSellPrice { get; set; }

		/// <summary>
		/// سریال
		/// </summary>
		public string Serial { get; set; }

		/// <summary>
		/// محل نگهداری( آدرس )
		/// </summary>
		public string Address { get; set; }
	}
}
