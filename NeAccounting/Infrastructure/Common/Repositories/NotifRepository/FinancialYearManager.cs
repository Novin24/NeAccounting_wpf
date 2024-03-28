using Domain.BaseDomain.FinancialYears;
using Infrastructure.Common.BaseRepositories;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;

namespace Infrastructure.Repositories
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
    }
}
