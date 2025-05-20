using Domain.Common;
using Domain.NovinEntity.Workers;
using DomainShared.Enums;
using DomainShared.Errore;

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
        public string? Receiver { get; set; }
        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; private set; }
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
            SetDesc(description);
            SubmitDate = submitDate;
            Expensetype = expensetype;
            Amount = amount;
            PayType = payType;
            Receiver = receiver;
        }
        #endregion


        #region Methods
        public Expense SetDesc(string? description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 200)
                throw new ArgumentException(NeErrorCodes.IsLess("توضیحات", "دویست"));

            Description = description;
            return this;
        }
        #endregion
    }
}
