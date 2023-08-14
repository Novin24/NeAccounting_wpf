using Application.Common;
using Domain.Enities.Notifications;
using DomainShared.Notifications;

namespace Application.IBaseRepositories
{
    public interface INotifManager : IBaseRepository<Notification>
    {
        Task<List<NotifViewModel>> GetNotifs();
    }
}
