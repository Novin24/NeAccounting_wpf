namespace DomainShared.ViewModels.Pun
{
	public class ImportMaterailListDto
	{
		public string MaterialName { get; set; }
		public string UnitName { get; set; }
		public Guid UnitId { get; set; }
		public long LastSellPrice { get; set; }
		public string Serial { get; set; }
		public string Address { get; set; }

	}
}
