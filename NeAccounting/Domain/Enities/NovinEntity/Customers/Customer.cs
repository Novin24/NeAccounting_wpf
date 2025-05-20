using Domain.Common;
using DomainShared.Enums;
using DomainShared.Errore;

namespace Domain.NovinEntity.Customers
{
    /// <summary>
    /// مشتری
    /// </summary>
    public class Customer : LocalEntity<Guid>
    {
        #region navigation
        #endregion

        #region properties
        /// <summary>
        /// نام
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// شماره مشتری
        /// </summary>
        public long CusId { get; set; }

        /// <summary>
        /// شماره موبایل
        /// </summary>
        public string Mobile { get; private set; }
        /// <summary>
        /// مجموع اعتبار
        /// </summary>
        public long TotalCredit { get; set; }
        /// <summary>
        /// مبلغ چک ضمانتی
        /// </summary>
        public long ChequeCredit { get; set; }
        /// <summary>
        /// ضمانت نقدی
        /// </summary>
        public long CashCredit { get; set; }

        /// <summary>
        /// ضمانت سفته
        /// </summary>
        public long PromissoryNote { get; set; }

        /// <summary>
        /// کدملی
        /// </summary>
        public string NationalCode { get; private set; }

        /// <summary>
        /// آدرس
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// خریدار
        /// </summary>
        public bool Buyer { get; set; }

        /// <summary>
        /// فروشنده
        /// </summary>
        public bool Seller { get; set; }

        /// <summary>
        /// نوع مشتری
        /// </summary>
        public CustomerType Type { get; set; }

        /// <summary>
        /// آیا مشتری چک گارانتی دارد؟
        /// </summary>
        public bool HaveChequeGuarantee { get; set; }

        /// <summary>
        /// آیا مشتری اعتبار نقدی دارد
        /// </summary>
        public bool HaveCashCredit { get; set; }

        /// <summary>
        /// آیا مشتری سفته دارد
        /// </summary>
        public bool HavePromissoryNote { get; set; }

        /// <summary>
        /// آیا فعال است؟
        /// </summary>
        public bool IsActive { get; set; }
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
            SetName(name);
            SetMobile(mobile);
            SetNationalCode(nationalCode);
            HaveChequeGuarantee = false;
            ChequeCredit = 0;
            TotalCredit = totalCredit;
            CashCredit = cashCredit;
            PromissoryNote = promissoryNote;
            HavePromissoryNote = havePromissoryNote;
            HaveCashCredit = haveCashCredit;
            Address = address;
            Buyer = isBuyer;
            Seller = isSeller;
            Type = type;
            IsActive = true;
        }

		/// <summary>
		/// اضافه کردن همه به سال مالی جدید
		/// </summary>
		/// <param name="cusList"></param>
		/// <returns></returns>
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
            bool isSeller,
            Guid id) : this(name, mobile, totalCredit, cashCredit, promissoryNote, nationalCode, address, type, havePromissoryNote, haveCashCredit, isBuyer, isSeller)
        {
            Id = id;
        }
        #endregion

        #region Method
        public Customer SetNationalCode(string nationalCode)
        {
            if (nationalCode.Length > 11)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("کد ملی", "یازده"));
            }
            NationalCode = nationalCode;
            return this;
        }

        public Customer SetName(string name)
        {
            if (name.Length > 50)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("نام مشتری", "پنجاه"));
            }
            Name = name;
            return this;
        }

        public Customer SetMobile(string mobile)
        {
            if (mobile.Length > 20)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("موبایل", "بیست"));
            }
            Mobile = mobile;
            return this;
        }

        public Customer SetAddress(string address)
        {
            if (address.Length > 150)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("آدرس", "صد و پنجاه"));
            }
            Address = address;
            return this;
        }
        #endregion
    }
}
