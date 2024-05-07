using Domain.BaseDomain.FinancialYears;
using DomainShared.Constants;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.PagedResul;
using NeApplication.Common;

namespace NeApplication.IBaseRepositories
{
    public interface IFinancialYearManager : IBaseRepository<FinancialYear>
    {
        
        Task<FiscalYearDtailsDto> GetActiveYear();
        Task<PagedResulViewModel<FiscalYearDto>> GetFiscalYears(
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);
        Task<(bool isSuccess, string error)> ChangeFinancialYear(Guid id);
        Task<bool> CheckNameExist(string name);
        Task<bool> CreateNewFinancialYear(string name, string databaseName, string description);
        Task<bool> CloseLastFinancialYear();

        Task<(bool isSuccess, string error)> CreateNewDatabase(string databaseName,
           string dFileName,
           string dLogFileName);
    }
}
