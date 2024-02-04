using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;

namespace DomainShared.ViewModels.Workers
{
    public struct SalaryViewModel
    {
        /// <summary>
        /// نام کارگر
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// کارکرد عادی
        /// </summary>
        public string AmountOf { get; set; }

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
        /// ماه مربوط به کارکرد
        /// </summary>
        public byte PersianMonth { get; set; }

        /// <summary>
        /// سال مربوط به کارکرد
        /// </summary>
        public int PersianYear{ get; set; }

        public readonly string DisplayDate
        {
            get
            {
                return string.Format("{0}/{1}", PersianYear, PersianMonth);
            }
        }

        public SalaryDetails Details { get; set; }
    }
}
