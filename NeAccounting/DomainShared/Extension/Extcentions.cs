using System.Globalization;

namespace DomainShared.Extension
{
    public static class Extcentions
    {
        public static string ToShamsiDate(this DateTime date, PersianCalendar pc)
        {
            return string.Concat(pc.GetYear(date), "/", pc.GetMonth(date), "/", pc.GetDayOfMonth(date));
        }
    }
}