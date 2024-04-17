using Domain.Enities.Notifications;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Extension;
using DomainShared.Notifications;
using DomainShared.Utilities;
using DomainShared.ViewModels.PagedResul;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;
using System.Globalization;

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

        public async Task<PagedResulViewModel<NotifViewModel>> GetNotifs(string titele, Priority priority, DateTime? startDate, DateTime? endDate, bool isInit, int pageNum = 0, int pageCount = NeAccountingConstants.PageCount)
        {
            var query = TableNoTracking
                .Where(t => string.IsNullOrEmpty(titele) || t.Titel.Contains(titele))
                .Where(t => priority == Priority.All || t.Priority == priority)
                .Where(t => !startDate.HasValue || t.DueDate.Date >= startDate.Value.Date)
                .Where(t => !endDate.HasValue || t.DueDate.Date < endDate.Value.Date)
                .Select(t => new NotifViewModel()
                {
                    Titele = t.Titel,
                    Id = t.Id,
                    DueDate = t.DueDate,
                    Priority = t.Priority,
                    Message = t.Message
                }).AsQueryable();

            var totalCount = await query.CountAsync();

            if (isInit && totalCount != 0)
            {
                pageNum = totalCount / pageCount;
                if (totalCount % pageCount != 0)
                {
                    pageNum++;
                }
            }
            var li = await query.OrderBy(t => t.DueDate).Skip((pageNum - 1) * pageCount).Take(pageCount).ToListAsync();

            PersianCalendar pc = new();
            foreach (var item in li)
            {
                item.ShamsiDueDate = item.DueDate.ToShamsiDate(pc);
                item.DisplayPriority = item.Priority.ToDisplay();
            }
            return new PagedResulViewModel<NotifViewModel>(totalCount, pageCount, pageNum, li);
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

        public async Task<(string error, bool isSuccess)> UpdateNotif(
            int id,
            string titele,
            string message,
            Priority priority,
            DateTime dueDate)
        {
            try
            {
                var mt = await Entities.FindAsync(id);

                if (mt == null)
                    return new("یاد آور مورد نظر یافت نشد !!!", false);
                mt.Titel = titele;
                mt.Message = message;
                mt.DueDate = dueDate;
                mt.Priority = priority;

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

        public async Task<(string error, bool isSuccess)> DeleteNotif(int docId)
        {
            try
            {
                var mt = await Entities.FindAsync(docId);

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
