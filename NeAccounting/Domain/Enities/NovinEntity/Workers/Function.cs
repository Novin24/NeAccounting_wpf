using Domain.Common;

namespace Domain.NovinEntity.Workers
{
    public class Function : LocalEntity
    {
        #region Navigation
        public int SalaryId { get; set; }
        public Salary Salary { get; set; }
        #endregion

        #region Property
        /// <summary>
        /// تاریخ کارکرد
        /// </summary>
        public DateTime SubmitDate { get; set; }

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
            DateTime submitDate,
            byte amountOf,
            byte amountOfOverTime,
            string? description)
        {
            SubmitDate = submitDate;
            AmountOf = amountOf;
            AmountOfOverTime = amountOfOverTime;
            Description = description;
        }
        #endregion
    }
}
