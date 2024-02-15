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
        public AidDetails Details { get; set; }
    }
}
