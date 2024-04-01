using Domain.Common;

namespace Domain.NovinEntity.Workers
{
    public class Salary : LocalEntity
    {
        #region Navigation
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        #endregion

        #region Property
        /// <summary>
        /// ماه شمسی فیش
        /// </summary>
        public byte PersianMonth { get; set; }

        /// <summary>
        /// سال شمسی فیش حقوقی
        /// </summary>
        public int PersianYear { get; set; }

        /// <summary>
        /// حقوق پرداختی
        /// </summary>
        public long AmountOf { get; set; }

        /// <summary>
        /// مساعده مالی
        /// </summary>
        public long FinancialAid { get; set; }

        /// <summary>
        /// اضافه کاری
        /// </summary>
        public long OverTime { get; set; }

        /// <summary>
        /// مالیات
        /// </summary>
        public long Tax { get; set; }

        /// <summary>
        /// حق اولاد
        /// </summary>
        public long ChildAllowance { get; set; }

        /// <summary>
        /// حق خوار و بار و مسکن
        /// </summary>
        public long RightHousingAndFood { get; set; }

        /// <summary>
        /// بیمه
        /// </summary>
        public long Insurance { get; set; }

        /// <summary>
        /// قسط وام 
        /// </summary>
        public long LoanInstallment { get; set; }

        /// <summary>
        /// سایر اضافات
        /// </summary>
        public long OtherAdditions { get; set; }

        /// <summary>
        /// سایر کسورات
        /// </summary>
        public long OtherDeductions { get; set; }

        /// <summary>
        /// باقی مانده
        /// </summary>
        public long LeftOver { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        #endregion

        #region Constructor
        public Salary()
        {
        }

        public Salary(
            int persianYear,
            byte persianMonth,
            long amountOf,
            long financialAid,
            long overTime,
            long tax,
            long childAllowance,
            long rightHousingAndFood,
            long insurance,
            long loanInstallment,
            long otherAdditions,
            long otherDeductions,
            long leftOver,
            string? description)
        {
            PersianMonth = persianMonth;
            PersianYear = persianYear;
            AmountOf = amountOf;
            FinancialAid = financialAid;
            OverTime = overTime;
            Tax = tax;
            ChildAllowance = childAllowance;
            Insurance = insurance;
            RightHousingAndFood = rightHousingAndFood;
            LoanInstallment = loanInstallment;
            OtherAdditions = otherAdditions;
            OtherDeductions = otherDeductions;
            LeftOver = leftOver;
            Description = description;
        }
        #endregion

        #region Methods

        #endregion
    }
}
