using Domain.Enities.Notifications;
using DomainShared.Enums;
using DomainShared.Notifications;
using Infrastructure.EntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Infrastructure.BaseRepositories
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

                Update(mt, false);
            }
            catch (Exception ex)
            {
                return new($"خطا دراتصال به پایگاه داده!!!\n {ex}", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> DeleteNotif(int id)
        {
            try
            {
                var mt = await Entities.FirstOrDefaultAsync(t => t.Id == id);

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
