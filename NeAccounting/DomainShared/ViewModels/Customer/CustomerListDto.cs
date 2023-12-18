using DomainShared.Enums;

namespace DomainShared.ViewModels.Customer
{
    public struct CustomerListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public uint TotalCredit { get; set; }
        public uint ChequeCredit { get; set; }
        public uint CashCredit { get; set; }
        public uint PromissoryNote { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public bool Buyer { get; set; }
        public bool Seller { get; set; }
        public CustomerType CusType { get; set; }
        public string CusTypeName { get; set; }
        public bool HaveChequeGuarantee { get; set; }
        public bool HaveCashCredit { get; set; }
        public bool HavePromissoryNote { get; set; }
    }
}
