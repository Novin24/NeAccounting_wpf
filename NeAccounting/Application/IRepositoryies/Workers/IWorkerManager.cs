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
            uint salary,
            uint overtimeSalary,
            uint shiftSalary,
            uint shiftOvertimeSalary,
            uint insurancePremium,
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
            uint salary,
            uint overtimeSalary,
            uint shiftSalary,
            uint shiftOvertimeSalary,
            uint insurancePremium,
            byte dayInMonth);
        #endregion

        #region Salary

        Task<(string error, bool isSuccess)> AddSalary(int workerId,
            byte submitMonth,
            int submitYaer,
            uint amountOf,
            uint financialAid,
            uint overTime,
            uint tax,
            uint childAllowance,
            uint rightHousingAndFood,
            uint insurance,
            uint loanInstallment,
            uint otherAdditions,
            uint otherDeductions,
            uint leftOver,
            string? description);


        Task<(string error, bool isSuccess)> UpdateSalary(int workerId,
            int salaryId,
            int persianYear,
            byte persianMonth,
            uint amountOf,
            uint financialAid,
            uint overTime,
            uint tax,
            uint childAllowance,
            uint rightHousingAndFood,
            uint insurance,
            uint loanInstallment,
            uint otherAdditions,
            uint otherDeductions,
            uint leftOver,
            string? description);


        Task<SalaryWorkerViewModel> GetSalaryDetailByWorkerId(int workerId, byte submitMonth, int submintYear);

        Task<SalaryWorkerViewModel> GetSalaryDetailBySalaryId(int workerId, int salaryId, byte persianMonth, int persianYear);

        Task<PagedResulViewModel<SalaryViewModel>> GetSalaryList(int? workerId,
             byte? startMonth,
             int? startYear,
             int? endMonth,
             int? endYear,
             int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> DeleteSalary(int workerId, int salaryId);
        #endregion

        #region func
        Task<(string error, bool isSuccess)> AddAid(
            int workerId,
            int persianYear,
            byte persianMonth,
            uint amountOf,
            string? description);

        Task<(string error, bool isSuccess)> UpdateAid(
            int workerId,
            int persianYear,
            byte persianMonth,
            int aidId,
            uint amountOf,
            string? description);

        Task<List<AidViewModel>> GetAidList(int workerId,
             int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(string error, bool isSuccess)> DeleteAid(int workerId,
            int persianYear,
            byte persianMonth,
            int aidId);
        #endregion

        #region aid
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
