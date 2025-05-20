using Domain.Common;
using DomainShared.Errore;

namespace Domain.NovinEntity.Workers
{
    /// <summary>
    /// مساعده
    /// </summary>
    public class FinancialAid : LocalEntity
    {
        #region Navigation
        public Personel Worker { get; set; }
        public Guid WorkerId { get; set; }
        #endregion

        #region Property
        /// <summary>
        /// تاریخ پرداخت مساعده
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// سال شمسی
        /// </summary>
        public int PersianYear { get; set; }

        /// <summary>
        /// ماه شمسی
        /// </summary>
        public byte PersianMonth { get; set; }

        /// <summary>
        /// مبلغ مساعده
        /// </summary>
        public long AmountOf { get; set; }


        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; private set; }

        #endregion

        #region Constructor
        public FinancialAid()
        {

        }

        public FinancialAid(
            DateTime submitDate,
            byte persianMonth,
            int persianYear,
            long amountOf,
            string? description)
        {
            SetDesc(description);
            SubmitDate = submitDate;
            PersianMonth = persianMonth;
            PersianYear = persianYear;
            AmountOf = amountOf;
        }
        #endregion

        #region Methods
        public FinancialAid SetDesc(string? description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 200)
                throw new ArgumentException(NeErrorCodes.IsLess("توضیحات", "دویست"));

            Description = description;
            return this;
        }
        #endregion
    }
}
