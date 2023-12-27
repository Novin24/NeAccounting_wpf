using Domain.Common;

namespace Domain.NovinEntity.Workers
{
    public class FinancialAid : LocalEntity
    {
        #region Navigation
        public int SalaryId { get; set; }
        public Salary Salary { get; set; }
        #endregion

        #region Property
        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public DateTime SubmitDate { get; set; }

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
            SubmitDate = submitDate;
            AmountOf = amountOf;
            Description = description;
        }
        #endregion
    }
}
