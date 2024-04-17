using DomainShared.Enums;

namespace DomainShared.Notifications
{
    public class NotifViewModel
    {
        public int Id { get; set; }
        public string Titele { get; set; }
        public string Message { get; set; }
        public DateTime DueDate { get; set; }
        public string ShamsiDueDate { get; set; }
        public Priority Priority { get; set; }
        public string DisplayPriority { get; set; }
    }
}
