using DomainShared.Extension;
using System.Globalization;

namespace DomainShared.ViewModels.Workers
{
    public struct AidViewModel
    {
        public string Name { get; set; }
        public long AmountPrice { get; set; }
        public int PersonelId { get; set; }
        public string Price { get; set; }
        public string? Description { get; set; }

        /// <summary>
        /// سال مربوط به کارکرد
        /// </summary>
        public DateTime SubmitDate{ get; set; }

        public readonly string DisplayDate
        {
            get
            {
                PersianCalendar pc = new();
                return SubmitDate.ToShamsiDate(pc);
            }
        }
        public AidDetails Details { get; set; }
    }
}
