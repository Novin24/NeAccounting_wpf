using Domain.Common;
using System.Globalization;

namespace Domain.NovinEntity.Workers
{
    public class Salary : LocalEntity
    {
        #region Navigation
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        /// <summary>
        /// لیست مساعده ها
        /// </summary>
        public ICollection<FinancialAid> Aids { get; private set; }

        /// <summary>
        /// لیست کارکرها
        /// </summary>
        public ICollection<Function> Functions { get; private set; }
        #endregion

        #region Property
        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// ماه شمسی فیش
        /// </summary>
        public int PersianMonth { get; set; }

        /// <summary>
        /// سال شمسی فیش حقوقی
        /// </summary>
        public int PersianYear { get; set; }

        /// <summary>
        /// حقوق پرداختی
        /// </summary>
        public uint AmountOf { get; set; }

        /// <summary>
        /// مساعده مالی
        /// </summary>
        public uint FinancialAid { get; set; }

        /// <summary>
        /// اضافه کاری
        /// </summary>
        public uint OverTime { get; set; }

        /// <summary>
        /// مالیات
        /// </summary>
        public uint Tax { get; set; }

        /// <summary>
        /// حق اولاد
        /// </summary>
        public uint ChildAllowance { get; set; }

        /// <summary>
        /// حق خوار و بار و مسکن
        /// </summary>
        public uint RightHousingAndFood { get; set; }

        /// <summary>
        /// بیمه
        /// </summary>
        public uint Insurance { get; set; }

        /// <summary>
        /// قسط وام 
        /// </summary>
        public uint LoanInstallment { get; set; }

        /// <summary>
        /// سایر اضافات
        /// </summary>
        public uint OtherAdditions { get; set; }

        /// <summary>
        /// سایر کسورات
        /// </summary>
        public uint OtherDeductions { get; set; }

        /// <summary>
        /// باقی مانده
        /// </summary>
        public uint LeftOver { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        #endregion

        #region Constructor
        public Salary()
        {
            Aids = new List<FinancialAid>();
        }

        public Salary(
            DateTime submitDate,
            uint amountOf,
            uint financialAid,
            uint overTime,
            uint tax,
            uint childAllowance,
            uint rightHousingAndFood,
            uint insurance,
            uint loanInstallment,
            uint otherAdditions,
            uint otherDeductions,
            uint leftOver,
            string? description)
        {
            PersianCalendar pc = new();
            PersianMonth = pc.GetMonth(submitDate);
            PersianYear = pc.GetYear(submitDate);
            SubmitDate = submitDate;
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
        public Salary AddFinancialAid(FinancialAid aid)
        {
            Aids.Add(aid);
            return this;
        }

        public Salary AddFunction(Function function)
        {
            Functions.Add(function);
            return this;
        }
        #endregion
    }
}
