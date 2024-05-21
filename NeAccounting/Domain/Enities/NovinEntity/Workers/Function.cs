using Domain.Common;
using DomainShared.Errore;

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
        public string? Description { get; private set; }

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
            SetDesc(description);
            PersianMonth = persianMonth;
            PersianYear = persianYear;
            AmountOf = amountOf;
            AmountOfOverTime = amountOfOverTime;
        }
        #endregion

        #region Methods
        public Function SetDesc(string? description)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 200)
                throw new ArgumentException(NeErrorCodes.IsLess("توضیحات", "دویست"));

            Description = description;
            return this;
        }
        #endregion
    }
}
