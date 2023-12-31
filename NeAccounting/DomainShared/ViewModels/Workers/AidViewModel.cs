using System.Globalization;

namespace DomainShared.ViewModels.Workers
{
    public struct AidViewModel
    {
        public string Name { get; set; }
        public uint AmountPrice { get; set; }
        public int PersonelId { get; set; }
        public string Price { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public readonly string DisplayDate
        {
            get
            {
                PersianCalendar pc = new();
                return string.Format("{0}/{1}/{2}", pc.GetYear(Date), pc.GetMonth(Date),pc.GetDayOfMonth(Date));
            }
        }
        public AidDetails Details { get; set; }
    }
}
