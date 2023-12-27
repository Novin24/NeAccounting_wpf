using DomainShared.Enums;

namespace DomainShared.ViewModels.Workers
{
    public struct WorkerVewiModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string NationalCode { get; set; }
        public string WorkerStatus { get; set; }

        /// <summary>
        /// موبایل
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// ادرس
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// تاریخ شروع به کار
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// تاریخ اتمام کار
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// شمار پرسنلی
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// شماره حساب
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// وضعیت
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// وضعیت شیفت
        /// </summary>
        public Shift Shift { get; set; }

        /// <summary>
        /// دستمزد
        /// </summary>
        public uint Salary { get; set; }

        /// <summary>
        /// دسمتزد اضافه کاری
        /// </summary>
        public uint OverTimeSalary { get; set; }

        /// <summary>
        /// حق بیمه
        /// </summary>
        public uint InsurancePremium { get; set; }

        /// <summary>
        /// تعداد روز کاری در ماه
        /// </summary>
        public byte DayInMonth { get; set; }

        /// <summary>
        /// دستمزد هر شیفت
        /// </summary>
        public uint ShiftSalary { get; set; }

        /// <summary>
        /// دسمتزد اضافه کاری شیفتی
        /// </summary>
        public uint ShiftOverTimeSalary { get; set; }
    }
}
