using Domain.Enities.Notifications;
using DomainShared.Notifications;
using Infrastructure.Common.BaseRepositories;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;

namespace Infrastructure.Repositories
{
    public class NotifManager : BaseRepository<Notification>, INotifManager
    {
        public NotifManager(BaseDomainDbContext context) : base(context) { }

        public async Task<List<NotifViewModel>> GetNotifs()
        {
            return await TableNoTracking.Where(x => x.DueDate < DateTime.Now.AddDays(3)).Select(t => new NotifViewModel()
            {
                Titele = t.Titel,
                Message = t.Message
            }).ToListAsync();
        }
    }
}
