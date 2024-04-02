using Domain.Common;
using DomainShared.Enums;

namespace Domain.NovinEntity.Expense
{
    public class Expense : LocalEntity<Guid>
    {
        #region Property


        /// <summary>
        /// تاریخ ثبت
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// نوع هزینه
        /// </summary>
        public string Expensetype { get; set; }
        /// <summary>
        /// مبلغ
        /// </summary>
        public long Amount { get; set; }
        /// <summary>
        /// نوع پرداخت
        /// </summary>
        public PaymentType PayType { get; set; }
        /// <summary>
        /// دریافت کننده
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string Description { get; set; }
        #endregion

        #region Constructor
        public Expense(
            DateTime submitDate,
            string expensetype,
            long amount,
            PaymentType payType,
            string receiver,
            string description
            )
        {
            SubmitDate = submitDate;
            Expensetype = expensetype;
            Amount = amount;
            PayType = payType;
            Receiver = receiver;
            Description = description;
        }
        #endregion

    }
}
