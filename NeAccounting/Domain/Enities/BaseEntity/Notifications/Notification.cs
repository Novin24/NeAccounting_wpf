using Domain.Common;
using Domain.Enities.NovinEntity.Remittances;
using DomainShared.Enums;
using DomainShared.Errore;

namespace Domain.Enities.Notifications
{
    public class Notification : BaseEntity
    {
        #region Navigation
        public Guid? DocumentId { get; set; }
        #endregion

        #region Property
        public string Titel { get; private set; }
        public string Message { get; private set; }
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
            SetTitel(titel);
            SetMessage(message);
            Priority = priority;
            DueDate = dueDate;
        }

        public Notification(
            string titel,
            string message,
            Priority priority,
            DateTime dueDate,
            Guid documentId) : this(titel, message, priority, dueDate)
        {
            DocumentId = documentId;
        }
        #endregion

        #region Methods
        public Notification SetTitel(string titel)
        {
            if (titel.Length > 50)
                throw new ArgumentException(NeErrorCodes.IsLess("عنوان", "صد"));

            Titel = titel;
            return this;
        }

        public Notification SetMessage(string message)
        {
            if (message.Length > 150)
                throw new ArgumentException(NeErrorCodes.IsLess("توضیحات", "صدوپنجاه"));

            Message = message;
            return this;
        }
        #endregion
    }
}
