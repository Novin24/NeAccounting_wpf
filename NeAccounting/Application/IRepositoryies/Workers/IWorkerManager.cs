using Domain.NovinEntity.Workers;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.ViewModels;
using DomainShared.ViewModels.PagedResul;
using DomainShared.ViewModels.Workers;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IWorkerManager : IRepository<Personel>
    {
        #region worker

        Task<List<PersonnerlSuggestBoxViewModel>> GetWorkers();

        Task<WorkerVewiModel> GetWorker(Guid workerId);

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
            Guid id,
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

        Task<(string error, bool isSuccess)> AddSalary(Guid workerId,
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


        Task<(string error, bool isSuccess)> UpdateSalary(Guid workerId,
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


        Task<SalaryWorkerViewModel> GetSalaryDetailByWorkerId(Guid workerId, byte submitMonth, int submintYear, int? salaryId = null);

        Task<(bool isSuccess, SalaryWorkerViewModel item)> GetSalaryDetailBySalaryId(Guid workerId, int salaryId, byte persianMonth, int persianYear);
        Task<(bool isSuccess, SalaryWorkerViewModel item)> GetSalaryDetailBySalaryId( int salaryId, byte persianMonth, int persianYear);

        Task<PagedResulViewModel<SalaryViewModel>> GetSalaryList(Guid? workerId,
             byte? startMonth,
             int? startYear,
             int? endMonth,
             int? endYear,
             int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> DeleteSalary(Guid workerId, int salaryId);
        #endregion

        #region aid
        Task<(string error, bool isSuccess)> AddAid(
            DateTime subDate,
            Guid workerId,
            int persianYear,
            byte persianMonth,
            long amountOf,
            string? description);

        Task<(string error, bool isSuccess)> UpdateAid(
            DateTime subDate,
            Guid workerId,
            int persianYear,
            byte persianMonth,
            int aidId,
            long amountOf,
            string? description);

        Task<List<AidViewModel>> GetAidList(Guid? workerId,
             int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> DeleteAid(Guid workerId,
            int persianYear,
            byte persianMonth,
            int aidId);
        #endregion

        #region func
        Task<(string error, bool isSuccess)> AddFunctuion(
           Guid workerId,
           int persianYear,
           byte persianMonth,
           byte amountOf,
           byte amountOfOverTime,
           string? description);

        Task<(string error, bool isSuccess)> UpdateFunc(
            Guid workerId,
            int persianYear,
            byte persianMonth,
            int funcId,
            byte amountOf,
            byte overTime,
            string? description);

        Task<List<FunctionViewModel>> GetFunctionList(Guid? workerId,
             int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> DeleteFunc(Guid workerId,
            int persianYear,
            byte persianMonth,
            int aidId);
        #endregion
    }
}
