using NeApplication.Common;
using Domain.Enities.Notifications;
using DomainShared.Notifications;
using DomainShared.Enums;
using DomainShared.Constants;
using DomainShared.ViewModels.PagedResul;

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

        Task<PagedResulViewModel<NotifViewModel>> GetNotifs(string titele, Priority priority, DateTime? startDate, DateTime? endDate, bool isInit, int pageNum = 0, int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> CreateNotif(
                string titel,
                string message,
                Priority priority,
                DateTime dueDate);

        Task<(string error, bool isSuccess)> UpdateNotif(
            Guid id,
            string message,
            DateTime dueDate);
        Task<(string error, bool isSuccess)> UpdateNotif(
            int id,
            string titele,
            string message,
            Priority priority,
            DateTime dueDate);

        Task<(string error, bool isSuccess)> DeleteNotif(Guid docId);

        Task<(string error, bool isSuccess)> DeleteNotif(int docId);

    }
}
