using Domain.Common;
using DomainShared.Enums;

namespace Domain.Enities.Notifications
{
    public class Notification : BaseEntity
    {

        #region ctor
        public Notification()
        {
        }
        #endregion

        #region Property
        public string Titel { get; set; }
        public string Message { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; } 
        #endregion
    }
}
