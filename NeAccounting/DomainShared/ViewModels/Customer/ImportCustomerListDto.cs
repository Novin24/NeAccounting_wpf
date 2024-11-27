using DomainShared.Enums;

namespace DomainShared.ViewModels.Customer
{
	public struct ImportCustomerListDto
	{
		public string Name { get; set; }
		public string NationalCode { get; set; }
		public CustomerType CusType { get; set; }
		public string CusTypeName { get; set; }
		public string Mobile { get; set; }
		public bool Buyer { get; set; }
		public bool Seller { get; set; }
		public string Address { get; set; }
	}
}
