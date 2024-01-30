using Domain.Common;

namespace Domain.NovinEntity.Workers
{
    public class FinancialAid : LocalEntity
    {
        #region Navigation
        public Worker Worker { get; set; }
        public int WorkerId { get; set; }
        #endregion

        #region Property
        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public DateTime PayDate { get; set; }

        /// <summary>
        /// سال شمسی
        /// </summary>
        public int PersianYear { get; set; }

        /// <summary>
        /// ماه شمسی
        /// </summary>
        public byte PersanMonth { get; set; }

        /// <summary>
        /// مبلغ مساعده
        /// </summary>
        public uint AmountOf { get; set; }


        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        #endregion

        #region Constructor
        public FinancialAid()
        {

        }

        public FinancialAid(
            DateTime submitDate,
            uint amountOf,
            string? description)
        {
            PayDate = submitDate;
            AmountOf = amountOf;
            Description = description;
        }
        #endregion
    }
}
