using DomainShared.Enums;

namespace DomainShared.ViewModels.Workers
{
    public struct SalaryWorkerViewModel
    {
        /// <summary>
        /// وضعیت شیفت
        /// </summary>
        public Shift ShiftStatus { get; set; }

        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public int PersonelId { get; set; }

        /// <summary>
        /// دستمزد
        /// </summary>
        public uint SalaryAmount { get; set; }

        /// <summary>
        /// دسمتزد اضافه کاری
        /// </summary>
        public uint OverTimeSalary { get; set; }

        /// <summary>
        /// حق بیمه
        /// </summary>
        public uint InsurancePremium { get; set; }

        /// <summary>
        /// مساعده
        /// </summary>
        public uint FinancialAidAmount { get; set; }

        public string Error{ get; set; }

        public bool Success { get; set; }
    }
}
