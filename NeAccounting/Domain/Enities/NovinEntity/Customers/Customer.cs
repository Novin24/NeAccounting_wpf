using Domain.Common;
using DomainShared.Enums;

namespace Domain.NovinEntity.Customers
{
    public class Customer : LocalEntity<Guid>
    {
        #region navigation

        #endregion

        #region ctor
        internal Customer() { }

        public Customer(
            string name,
            string mobile,
            uint totalCredit,
            uint chequeCredit,
            uint cashCredit,
            uint promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveChequeGuarantee,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller)
        {
            Name = name;
            Mobile = mobile;
            TotalCredit = totalCredit;
            ChequeCredit = chequeCredit;
            CashCredit = cashCredit;
            PromissoryNote = promissoryNote;
            NationalCode = nationalCode;
            HavePromissoryNote = havePromissoryNote;
            HaveCashCredit = haveCashCredit;
            HaveChequeGuarantee = haveChequeGuarantee;
            Address = address;
            Buyer = isBuyer;
            Seller = isSeller;
            Type = type;

        }
        #endregion

        #region properties
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
        public CustomerType Type { get; set; }
        public bool HaveChequeGuarantee { get; set; }
        public bool HaveCashCredit { get; set; }
        public bool HavePromissoryNote { get; set; }
        #endregion
    }
}
