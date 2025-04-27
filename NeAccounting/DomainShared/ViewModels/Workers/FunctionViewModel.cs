using System.Globalization;

namespace DomainShared.ViewModels.Workers
{
    public struct FunctionViewModel
    {
        /// <summary>
        /// نام کارگر
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// کارکرد عادی
        /// </summary>
        public byte Amountof { get; set; }

        /// <summary>
        /// کارکرد اضافه کاری
        /// </summary>
        public byte OverTime { get; set; }

        /// <summary>
        /// شماره پرسنلی
        /// </summary>
        public int PersonelId { get; set; }

        /// <summary>
        /// توضیحات
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// ماه مربوط به کارکرد
        /// </summary>
        public byte PersianMonth { get; set; }

        /// <summary>
        /// سال مربوط به کارکرد
        /// </summary>
        public int PersianYear { get; set; }

        public readonly string DisplayDate
        {
            get
            {
                return string.Format("{0}/{1}", PersianYear, PersianMonth);
            }
        }
        public FucntionDetails Details { get; set; }

        /// <summary>
        /// آیا دارای فیش حقوقی هست؟
        /// </summary>
        public bool HasSalary { get; set; }
    }
}
