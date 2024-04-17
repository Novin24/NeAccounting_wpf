using Domain.Enities.Notifications;
using DomainShared.Enums;
using DomainShared.Notifications;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;

namespace Infrastructure.BaseRepositories
{
    public class NotifManager(BaseDomainDbContext context) : BaseRepository<Notification>(context), INotifManager
    {
        public async Task<List<NotifViewModel>> GetNotifs()
        {
            return await TableNoTracking
                .Where(t => t.Priority == Priority.Low && t.DueDate.Date == DateTime.Now.Date ||
                t.Priority == Priority.Medium && t.DueDate.Date >= DateTime.Now.Date && t.DueDate.Date < DateTime.Now.Date.AddDays(3) ||
                t.Priority == Priority.High && t.DueDate.Date >= DateTime.Now.Date.AddDays(-1) && t.DueDate.Date < DateTime.Now.Date.AddDays(4))
                .Select(t => new NotifViewModel()
                {
                    Titele = t.Titel,
                    Message = t.Message
                }).ToListAsync();
        }

        public async Task<(string error, bool isSuccess)> CreateNotif(
            Guid docId,
            string titel,
            string message,
            Priority priority,
            DateTime dueDate)
        {

            try
            {
                var t = await Entities.AddAsync(new Notification(docId,
                    titel,
                    message,
                    priority,
                    dueDate));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> CreateNotif(
            string titel,
            string message,
            Priority priority,
            DateTime dueDate)
        {
            try
            {
                var t = await Entities.AddAsync(new Notification(
                    titel,
                    message,
                    priority,
                    dueDate));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateNotif(
            Guid id,
            string message,
            DateTime dueDate)
        {
            try
            {
                var mt = await Entities.FirstOrDefaultAsync(t => t.DocumentId == id);

                if (mt == null)
                    return new("یاد آور مورد نظر یافت نشد !!!", false);

                mt.Message = message;
                mt.DueDate = dueDate;

                Entities.Update(mt);
            }
            catch (Exception ex)
            {
                return new($"خطا دراتصال به پایگاه داده!!!\n {ex}", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> DeleteNotif(Guid docId)
        {
            try
            {
                var mt = await Entities.FirstOrDefaultAsync(t => t.DocumentId == docId);

                if (mt == null)
                    return new("یاد آور مورد نظر یافت نشد !!!", false);

                Entities.Remove(mt);
            }
            catch (Exception ex)
            {
                return new($"خطا دراتصال به پایگاه داده!!!\n {ex}", false);
            }
            return new(string.Empty, true);
        }
    }
}
