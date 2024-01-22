using System.Globalization;

namespace DomainShared.ViewModels.Workers
{
    public struct SalaryViewModel
    {
        /// <summary>
        /// نام کارگر
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// کارکرد عادی
        /// </summary>
        public string Amountof { get; set; }

        /// <summary>
        /// کارکرد اضافه کاری
        /// </summary>
        public string OverTime { get; set; }

        /// <summary>
        /// ممجموع بدهکاری
        /// </summary>
        public string TotalDebt { get; set; }

        /// <summary>
        /// باقی مانده
        /// </summary>
        public string LeftOver { get; set; }

        /// <summary>
        /// تاریخ مربوط به کارکرد
        /// </summary>
        public DateTime Date { get; set; }

        public readonly string DisplayDate
        {
            get
            {
                PersianCalendar pc = new();
                return string.Format("{0}/{1}/{2}", pc.GetYear(Date), pc.GetMonth(Date),pc.GetDayOfMonth(Date));
            }
        }
        public SalaryDetails Details { get; set; }
    }
}
