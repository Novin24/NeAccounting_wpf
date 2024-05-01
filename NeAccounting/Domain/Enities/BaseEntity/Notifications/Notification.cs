using Domain.Common;
using DomainShared.Enums;

namespace Domain.Enities.Notifications
{
    public class Notification : BaseEntity
    {
        #region Navigation
        public Guid? DocumentId { get; set; }
        #endregion

        #region Property
        public string Titel { get; set; }
        public string Message { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        #endregion

        #region ctor
        public Notification()
        {
        }
        public Notification(
            string titel,
            string message,
            Priority priority,
            DateTime dueDate)
        {
            Titel = titel;
            Message = message;
            Priority = priority;
            DueDate = dueDate;
        }

        public Notification(
            Guid documentId,
            string titel,
            string message,
            Priority priority,
            DateTime dueDate)
        {
            DocumentId = documentId;
            Titel = titel;
            Message = message;
            Priority = priority;
            DueDate = dueDate;
        }
        #endregion
    }
}
