using System.Globalization;

namespace DomainShared.ViewModels.Workers
{
    public struct AidViewModel
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public DateTime Date
        {
            set
            {
                PersianCalendar pc = new();
                DisplayDate = string.Format("{0}/{1}/{2}", pc.GetYear(value), pc.GetMonth(value), pc.GetDayOfMonth(value));
            }
        }
        public string DisplayDate { get; private set; }
        public AidDetails Details { get; set; }
    }

    public struct AidDetails
    {
        public int Id { get; set; }
        public int SalaryId { get; set; }
        public int WorkerId { get; set; }
    }
}
