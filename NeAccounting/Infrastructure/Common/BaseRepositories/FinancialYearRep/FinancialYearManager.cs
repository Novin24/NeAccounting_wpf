using Domain.BaseDomain.FinancialYears;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;

namespace Infrastructure.BaseRepositories
{
    public class FinancialYearManager : BaseRepository<FinancialYear>, IFinancialYearManager
    {
        public FinancialYearManager(BaseDomainDbContext context) : base(context) { }

        public async Task<(bool isSuccess, string databaseName)> GetActiveYear()
        {
            var t = await TableNoTracking.FirstOrDefaultAsync(t => t.IsActive);
            if (t == null)
            {
                return new(false, string.Empty);
            }
            return new(true, t.DataBaseName);
        }

        public async Task<bool> CreateNewFinancialYear(string name, string databaseName, string description)
        {
            //var g = Guid.NewGuid().ToString().Replace("-", "")[15..];
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
    }
}
