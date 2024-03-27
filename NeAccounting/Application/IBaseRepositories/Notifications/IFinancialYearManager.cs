using Domain.BaseDomain.FinancialYears;
using NeApplication.Common;

namespace NeApplication.IBaseRepositories
{
    public interface IFinancialYearManager : IBaseRepository<FinancialYear>
    {
        Task<(bool isSuccess, string databaseName)> GetActiveYear();
    }
}
