using NeApplication.Common;
using Domain.Enities.Notifications;
using DomainShared.Notifications;

namespace NeApplication.IBaseRepositories
{
    public interface INotifManager : IBaseRepository<Notification>
    {
        Task<List<NotifViewModel>> GetNotifs();
    }
}
