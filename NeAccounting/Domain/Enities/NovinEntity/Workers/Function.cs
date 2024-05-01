using Domain.Common;

namespace Domain.NovinEntity.Workers
{
    public class Function : LocalEntity
    {
        #region Navigation
        public Personel Worker { get; set; }
        public Guid WorkerId { get; set; }
        #endregion

        #region Property
        /// <summary>
        /// سال شمسی
        /// </summary>
        public int PersianYear { get; set; }

        /// <summary>
        /// ماه شمسی
        /// </summary>
        public byte PersianMonth { get; set; }

        /// <summary>
        /// تعداد  کاری
        /// </summary>
        public byte AmountOf { get; set; }

        /// <summary>
        /// تعداد اضافه کاری
        /// </summary>
        public byte AmountOfOverTime { get; set; }


        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        #endregion

        #region Constructor
        public Function()
        {

        }

        public Function(
            byte persianMonth,
            int persianYear,
            byte amountOf,
            byte amountOfOverTime,
            string? description)
        {
            PersianMonth = persianMonth;
            PersianYear = persianYear;
            AmountOf = amountOf;
            AmountOfOverTime = amountOfOverTime;
            Description = description;
        }
        #endregion
    }
}
