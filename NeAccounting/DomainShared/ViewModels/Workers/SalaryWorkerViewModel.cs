using DomainShared.Enums;

namespace DomainShared.ViewModels.Workers
{
    public class SalaryWorkerViewModel
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
        /// ماه پرداخت
        /// </summary>
        public byte SubmitMonth { get; set; }
        
        /// <summary>
        /// سال پرداخت
        /// </summary>
        public int SubmitYear{ get; set; }

        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public int PersonelId { get; set; }
        
        /// <summary>
        /// تعداد روز یا ساعت کاری
        /// </summary>
        public int FunctionNum { get; set; }
        
        /// <summary>
        /// تعداد اضافه کاری
        /// </summary>
        public int OverTimeNum { get; set; }

        /// <summary>
        /// کارکرد عادی
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


        public string Error { get; set; }

        public bool Success { get; set; }
    }
}
