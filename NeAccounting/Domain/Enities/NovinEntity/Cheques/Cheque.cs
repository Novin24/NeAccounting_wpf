using Domain.Common;
using Domain.NovinEntity.Documents;
using DomainShared.Enums;

namespace Domain.NovinEntity.Cheques
{
    public class Cheque : LocalEntity<Guid>
    {
        #region Navigation
        public Guid DocumetnId { get; set; }
        public Document Document { get; set; }
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
        /// شماره چک
        /// </summary>
        public string Cheque_Number { get; set; }

        /// <summary>
        /// سریال دیتابیسی
        /// </summary>
        public long Serial { get; set; }

        /// <summary>
        /// شماره حساب
        /// </summary>
        public string Accunt_Number { get; set; }

        /// <summary>
        /// نام بانک
        /// </summary>
        public string Bank_Name { get; set; }

        /// <summary>
        /// نام شعبه
        /// </summary>
        public string Bank_Branch { get; set; }

        /// <summary>
        /// صاحب چک
        /// </summary>
        public string Cheque_Owner { get; set; }
        #endregion

        #region Constructor
        internal Cheque()
        {

        }

        public Cheque(SubmitChequeStatus submitStatus,
            ChequeStatus chequeStatus,
            DateTime? dueDate,
            string cheque_Number,
            string accunt_Number,
            string bank_Name,
            string bank_Branch,
            string cheque_Owner)
        {
            Bank_Name = bank_Name;
            Bank_Branch = bank_Branch;
            Due_Date = dueDate;
            Cheque_Number = cheque_Number;
            Accunt_Number = accunt_Number;
            Cheque_Owner = cheque_Owner;
            SubmitStatus = submitStatus;
            Status = chequeStatus;
        }
        #endregion
    }
}
