using Domain.Common;
using Domain.NovinEntity.Documents;
using DomainShared.Enums;
using DomainShared.Errore;

namespace Domain.NovinEntity.Cheques
{
    /// <summary>
    /// چک
    /// </summary>
    public class Cheque : LocalEntity<Guid>
    {
        #region Navigation
        public Guid DocumetnId { get; set; }
        public Document Document { get; set; }

        public Guid Payer { get; set; }
        public Guid Reciver { get; set; }
        #endregion

        #region Property

        /// <summary>
        /// وضعیت ثبت
        /// </summary>
        public SubmitChequeStatus SubmitStatus { get; set; }

        /// <summary>
        /// وضعیت چک
        /// </summary>
        public ChequeStatus Status { get; set; }

        /// <summary>
        /// تاریخ واگذاری
        /// </summary>
        public DateTime? TransferdDate { get; set; }

        /// <summary>
        /// تاریخ سر رسید
        /// </summary>
        public DateTime? Due_Date { get; set; }

		/// <summary>
		/// سریال چک
		/// </summary>
		public string Cheque_Number { get; private set; }

		/// <summary>
		/// سری چک
		/// </summary>
		public string? Cheque_Series { get; private set; }

		/// <summary>
		/// شماره صیادی
		/// </summary>
		public string? SiadyNumber { get; private set; }

		/// <summary>
		/// سریال دیتابیسی
		/// </summary>
		public long Serial { get; set; }

        /// <summary>
        /// شماره شبا
        /// </summary>
        public string? Accunt_Number { get; private set; }

        /// <summary>
        /// نام بانک
        /// </summary>
        public string Bank_Name { get; private set; }

        /// <summary>
        /// نام شعبه
        /// </summary>
        public string? Bank_Branch { get; private set; }

        /// <summary>
        /// صاحب چک
        /// </summary>
        public string Cheque_Owner { get; private set; }
        #endregion

        #region Constructor
        internal Cheque()
        {

        }

        public Cheque(SubmitChequeStatus submitStatus,
            ChequeStatus chequeStatus,
            Guid reciver,
            Guid payer,
            DateTime? dueDate,
			string cheque_Number,
			string? cheque_Series,
			string? siadyNumber,
			string accunt_Number,
            string bank_Name,
            string bank_Branch,
            string cheque_Owner)
        {
            SetBank_Name(bank_Name);
            SetBank_Branch(bank_Branch);
			SetAccunt_Number(accunt_Number);
			SetCheque_Owner(cheque_Owner);
            SetCheque_Number(cheque_Number);
			SetCheque_Series(cheque_Series);
            SetSiadyNumber(siadyNumber);
			Payer = payer;
            Reciver = reciver;
            Due_Date = dueDate;
            SubmitStatus = submitStatus;
            Status = chequeStatus;
        }

        public Cheque SetBank_Branch(string? bank_Branch)
        {
            if (!string.IsNullOrEmpty(bank_Branch) && bank_Branch.Length > 50)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("شعبه بانک", "پنجاه"));
            }
            Bank_Branch = bank_Branch;
            return this;
        }

        public Cheque SetBank_Name(string bank_Name)
        {
            if (bank_Name.Length > 50)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("نام بانک", "پنجاه"));
            }
            Bank_Name = bank_Name;
            return this;
        }

        public Cheque SetAccunt_Number(string? accunt_Number)
        {
            if (!string.IsNullOrEmpty(accunt_Number) && accunt_Number.Length > 100)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("شماره حساب", "صد"));
            }
            Accunt_Number = accunt_Number;
            return this;
        }

        public Cheque SetCheque_Owner(string cheque_Owner)
        {
            if (cheque_Owner.Length > 50)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("صاحب چک", "پنجاه"));
            }
            Cheque_Owner = cheque_Owner;
            return this;
        }

        public Cheque SetCheque_Number(string cheque_Number)
        {
            if (cheque_Number.Length > 100)
            {
                throw new ArgumentException(NeErrorCodes.IsLess("سریال چک", "صد"));
            }
            Cheque_Number = cheque_Number;
            return this;
		}
		public Cheque SetCheque_Series(string? cheque_Series)
		{
			if (!string.IsNullOrEmpty(cheque_Series) && cheque_Series.Length > 100)
			{
				throw new ArgumentException(NeErrorCodes.IsLess("سری چک", "صد"));
			}
			Cheque_Series = cheque_Series;
			return this;
		}
		public Cheque SetSiadyNumber(string? siadyNumber)
		{
			if (!string.IsNullOrEmpty(siadyNumber) && siadyNumber.Length > 100)
			{
				throw new ArgumentException(NeErrorCodes.IsLess("سری چک", "صد"));
			}
			SiadyNumber = siadyNumber;
			return this;
		}
		#endregion
	}
}
