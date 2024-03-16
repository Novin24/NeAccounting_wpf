using Domain.Common;
using Domain.NovinEntity.Customers;
using Domain.NovinEntity.Documents;
using DomainShared.Enums.Cheque;

namespace Domain.NovinEntity.Cheques
{
    public class Cheque : LocalEntity<Guid>
    {
        #region Navigation
        public Guid PayerId { get; set; }
        public Customer Payer { get; set; }
        public Guid ReciverId { get; set; }
        public Customer Reciver { get; set; }
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
        /// مبلغ چک
        /// </summary>
        public long Price { get; set; }

        /// <summary>
        /// تاریخ واگذاری
        /// </summary>
        public DateTime TransferdDate { get; set; }

        /// <summary>
        /// تاریخ دریافت یا پرداخت
        /// </summary>
        public DateTime RecivedOrPayDate { get; set; }

        /// <summary>
        /// تاریخ سر رسید
        /// </summary>
        public DateTime Due_Date { get; set; }

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

        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Constructor
        public Cheque(string bank_Name,
            string bank_Branch,
            string cheque_Number,
            int price,
            string accunt_Number,
            string cheque_Owner,
            SubmitChequeStatus submitStatus,
            ChequeStatus chequeStatus,
            string description)
        {
            Bank_Name = bank_Name;
            Bank_Branch = bank_Branch;
            Cheque_Number = cheque_Number;
            Price = price;
            Accunt_Number = accunt_Number;
            Cheque_Owner = cheque_Owner;
            SubmitStatus = submitStatus;
            Status = chequeStatus;
            Description = description;
        }
        #endregion

    }
}
