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
        public FucntionDetails Details { get; set; }
    }
}
