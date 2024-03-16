using Domain.Common;
using Domain.NovinEntity.Cheques;
using DomainShared.Enums;

namespace Domain.NovinEntity.Customers
{
    public class Customer : LocalEntity<Guid>
    {
        #region navigation
        public List<Cheque> RecCheque { get; set; }
        public List<Cheque> PayCheque { get; set; }
        #endregion

        #region ctor
        internal Customer() { }

        public Customer(
            string name,
            string mobile,
            long totalCredit,
            long cashCredit,
            long promissoryNote,
            string nationalCode,
            string address,
            CustomerType type,
            bool havePromissoryNote,
            bool haveCashCredit,
            bool isBuyer,
            bool isSeller)
        {
            HaveChequeGuarantee = false;
            ChequeCredit = 0;
            Name = name;
            Mobile = mobile;
            TotalCredit = totalCredit;
            CashCredit = cashCredit;
            PromissoryNote = promissoryNote;
            NationalCode = nationalCode;
            HavePromissoryNote = havePromissoryNote;
            HaveCashCredit = haveCashCredit;
            Address = address;
            Buyer = isBuyer;
            Seller = isSeller;
            Type = type;
            IsActive = true;
        }
        #endregion

        #region properties
        public string Name { get; set; }
        public long CusId { get; set; }
        public string Mobile { get; set; }
        public long TotalCredit { get; set; }
        public long ChequeCredit { get; set; }
        public long CashCredit { get; set; }
        public long PromissoryNote { get; set; }
        public string NationalCode { get; set; }
        public string Address { get; set; }
        public bool Buyer { get; set; }
        public bool Seller { get; set; }
        public CustomerType Type { get; set; }
        public bool HaveChequeGuarantee { get; set; }
        public bool HaveCashCredit { get; set; }
        public bool HavePromissoryNote { get; set; }
        public bool IsActive{ get; set; }
        #endregion
    }
}
