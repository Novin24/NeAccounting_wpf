using Domain.BaseDomain.FinancialYears;
using DomainShared.Constants;
using DomainShared.Extension;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.PagedResul;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;
using System.Globalization;

namespace Infrastructure.BaseRepositories
{
    public class FinancialYearManager(BaseDomainDbContext context) : BaseRepository<FinancialYear>(context), IFinancialYearManager
    {
        public async Task<FiscalYearDtailsDto> GetActiveYear()
        {
            var t = await TableNoTracking.FirstOrDefaultAsync(t => t.IsActive);
            if (t == null)
            {
                return new FiscalYearDtailsDto() { isSucces = false };
            }
            return new FiscalYearDtailsDto()
            {
                isSucces = true,
                databaseName = t.DataBaseName,
                databaseTitle = t.Name,
                isCurrent = t.IsCurrent
            };
        }

        public async Task<PagedResulViewModel<FiscalYearDto>> GetFiscalYears(
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            PersianCalendar pc = new();
            var query = TableNoTracking.Select(t => new FiscalYearDto
            {
                EndDate = t.EndDate,
                Id = t.Id,
                Titele = t.Name,
                StartDate = t.StartDate,
                Des = t.Descripion,
                SStartDate = t.StartDate.ToShamsiDate(pc),
                SEndDate = t.EndDate.ToShamsiDate(pc),
                NotActive = !t.IsActive,
                IsCurrent = t.IsCurrent
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
            var li = await query.OrderBy(t => t.StartDate).Skip((pageNum - 1) * pageCount).Take(pageCount).ToListAsync();

            for (int i = 1; i <= li.Count; i++)
            {
                li[i - 1].Row = i;
            }

            return new PagedResulViewModel<FiscalYearDto>(totalCount, pageCount, pageNum, li);
        }

        public async Task<bool> CreateNewFinancialYear(string name, string databaseName, string description)
        {
            var fi = new FinancialYear(name, databaseName, description);
            try
            {
                await Entities.AddAsync(fi);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CloseLastFinancialYear()
        {
            var t = await Entities.FirstOrDefaultAsync(t => t.IsActive);
            if (t == null) return false;

            t.EndDate = DateTime.Now;
            t.IsActive = false;
            t.IsCurrent = false;
            try
            {
                Entities.Update(t);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public async Task<(bool isSuccess, string error)> ChangeFinancialYear(Guid id)
        {
            var t = await Entities.FirstOrDefaultAsync(t => t.IsActive);
            var v = await Entities.FirstOrDefaultAsync(t => t.Id == id);

            if (t == null || v == null) return new(false, "خطا در یافتن پایگاه داده");

            t.IsActive = false;
            v.IsActive = true;

            NeAccountingConstants.NvoinDbConnectionStrint = v.DataBaseName;
            NeAccountingConstants.ReadOnlyMode = !v.IsCurrent;
            try
            {
                Entities.Update(t);
            }
            catch
            {
                return new(false, " خطا در اتصال به پایگاه داده code(37t46993)");
            }
            return new(true, string.Empty);
        }
    }
}
