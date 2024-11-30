using DomainShared.Enums;

namespace DomainShared.ViewModels.Customer
{
	public struct CustomerListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public long UniqNumber { get; set; }
        public string Mobile { get; set; }
        public long TTotalCredit { get; set; }
        public string TotalCredit { get; set; }
        public long ChequeCredit { get; set; }
        public long CashCredit { get; set; }
        public long PromissoryNote { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public bool Buyer { get; set; }
        public bool Seller { get; set; }
        public CustomerType CusType { get; set; }
        public string CusTypeName { get; set; }
        public bool HaveChequeGuarantee { get; set; }
        public bool HaveCashCredit { get; set; }
        public bool HavePromissoryNote { get; set; }
        public bool IsActive { get; set; }
	}
}
