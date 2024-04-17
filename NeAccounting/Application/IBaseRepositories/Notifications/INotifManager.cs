using NeApplication.Common;
using Domain.Enities.Notifications;
using DomainShared.Notifications;
using DomainShared.Enums;

namespace NeApplication.IBaseRepositories
{
    public interface INotifManager : IBaseRepository<Notification>
    {
        Task<List<NotifViewModel>> GetNotifs();
        Task<(string error, bool isSuccess)> CreateNotif(
                   Guid docId,
                   string titel,
                   string message,
                   Priority priority,
                   DateTime dueDate);

        Task<(string error, bool isSuccess)> CreateNotif(
                string titel,
                string message,
                Priority priority,
                DateTime dueDate);

        Task<(string error, bool isSuccess)> UpdateNotif(
            Guid id,
            string message,
            DateTime dueDate);

        Task<(string error, bool isSuccess)> DeleteNotif(Guid docId);
    }
}
