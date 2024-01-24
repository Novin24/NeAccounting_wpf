using DomainShared.Enums;

namespace DomainShared.ViewModels.Workers
{
    public struct SalaryWorkerViewModel
    {
        /// <summary>
        /// شیفت کاری
        /// </summary>
        public Shift ShiftStatus { get; set; }
        /// <summary>
        /// نام کارگر
        /// </summary>
        public string WorkerName { get; set; }

        /// <summary>
        /// تاریخ پرداخت
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public int PersonelId { get; set; }
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


        public string Error{ get; set; }

        public bool Success { get; set; }
    }
}
