using Domain.NovinEntity.Workers;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.ViewModels;
using DomainShared.ViewModels.PagedResul;
using DomainShared.ViewModels.Workers;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IWorkerManager : IRepository<Worker>
    {
        #region worker

        Task<List<PersonnerlSuggestBoxViewModel>> GetWorkers();

        Task<WorkerVewiModel> GetWorker(int workerId);

        Task<List<WorkerVewiModel>> GetWorkers(string fullName,
            string jobTitle,
            string nationalCode,
            Status status,
             int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> Create(
            string fullName,
            string natinalCode,
            string mobile,
            string address,
            int personalId,
            string accountNumber,
            string description,
            string jobTitle,
            DateTime startDate,
            Shift shift,
            long salary,
            long overtimeSalary,
            long shiftSalary,
            long shiftOvertimeSalary,
            long insurancePremium,
            byte dayInMonth);

        Task<(string error, bool isSuccess)> Update(
            int id,
            string fullName,
            string natinalCode,
            string mobile,
            string address,
            DateTime startDate,
            int personalId,
            string accountNumber,
            string description,
            string jobTitle,
            Status status,
            Shift shift,
            long salary,
            long overtimeSalary,
            long shiftSalary,
            long shiftOvertimeSalary,
            long insurancePremium,
            byte dayInMonth);
        #endregion

        #region Salary

        Task<(string error, bool isSuccess)> AddSalary(int workerId,
            byte submitMonth,
            int submitYaer,
            long amountOf,
            long financialAid,
            long overTime,
            long tax,
            long childAllowance,
            long rightHousingAndFood,
            long insurance,
            long loanInstallment,
            long otherAdditions,
            long otherDeductions,
            long leftOver,
            string? description);


        Task<(string error, bool isSuccess)> UpdateSalary(int workerId,
            int salaryId,
            int persianYear,
            byte persianMonth,
            long amountOf,
            long financialAid,
            long overTime,
            long tax,
            long childAllowance,
            long rightHousingAndFood,
            long insurance,
            long loanInstallment,
            long otherAdditions,
            long otherDeductions,
            long leftOver,
            string? description);


        Task<SalaryWorkerViewModel> GetSalaryDetailByWorkerId(int workerId, byte submitMonth, int submintYear, int? salaryId = null);

        Task<(bool isSuccess, SalaryWorkerViewModel item)> GetSalaryDetailBySalaryId(int workerId, int salaryId, byte persianMonth, int persianYear);
        Task<(bool isSuccess, SalaryWorkerViewModel item)> GetSalaryDetailBySalaryId( int salaryId, byte persianMonth, int persianYear);

        Task<PagedResulViewModel<SalaryViewModel>> GetSalaryList(int? workerId,
             byte? startMonth,
             int? startYear,
             int? endMonth,
             int? endYear,
             int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> DeleteSalary(int workerId, int salaryId);
        #endregion

        #region aid
        Task<(string error, bool isSuccess)> AddAid(
            DateTime subDate,
            int workerId,
            int persianYear,
            byte persianMonth,
            long amountOf,
            string? description);

        Task<(string error, bool isSuccess)> UpdateAid(
            DateTime subDate,
            int workerId,
            int persianYear,
            byte persianMonth,
            int aidId,
            long amountOf,
            string? description);

        Task<List<AidViewModel>> GetAidList(int workerId,
             int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> DeleteAid(int workerId,
            int persianYear,
            byte persianMonth,
            int aidId);
        #endregion

        #region func
        Task<(string error, bool isSuccess)> AddFunctuion(
           int workerId,
           int persianYear,
           byte persianMonth,
           byte amountOf,
           byte amountOfOverTime,
           string? description);

        Task<(string error, bool isSuccess)> UpdateFunc(
            int workerId,
            int persianYear,
            byte persianMonth,
            int funcId,
            byte amountOf,
            byte overTime,
            string? description);

        Task<List<FunctionViewModel>> GetFunctionList(int workerId,
             int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> DeleteFunc(int workerId,
            int persianYear,
            byte persianMonth,
            int aidId);
        #endregion
    }
}
