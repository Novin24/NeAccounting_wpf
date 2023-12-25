using Domain.Common;
using DomainShared.Enums;

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
        /// تاریخ پرداخت
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// تعداد روز / شیفت
        /// </summary>
        public int AmountOf { get; set; }

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
            DateTime submitDate,
            int amountOf,
            uint financialAid,
            uint overTime,
            uint tax,
            uint childAllowance,
            uint rightHousingAndFood,
            uint insurance,
            uint loanInstallment,
            uint otherAdditions,
            uint otherDeductions,
            long leftOver,
            string? description)
        {
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
    }
}
