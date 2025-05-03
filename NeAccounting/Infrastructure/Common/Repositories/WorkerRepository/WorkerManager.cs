using Domain.NovinEntity.Workers;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.Utilities;
using DomainShared.ViewModels;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.PagedResul;
using DomainShared.ViewModels.Workers;
using Infrastructure.EntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NeApplication.IRepositoryies;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Infrastructure.Repositories
{
    public class WorkerManager(NovinDbContext context) : Repository<Personel>(context), IWorkerManager
    {

        #region sp
        private DbCommand CreateCommand(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            var command = DbContext.Database.GetDbConnection().CreateCommand();
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.Transaction = DbContext.Database.CurrentTransaction?.GetDbTransaction();
            command.CommandTimeout = 20;
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }

        private async Task EnsureConnectionOpenAsync(CancellationToken cancellationToken = default)
        {
            var connection = DbContext.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }
        }
        #endregion

        #region Worker

        public Task<List<PersonnerlSuggestBoxViewModel>> GetWorkers()
        {
            return TableNoTracking.Select(x => new PersonnerlSuggestBoxViewModel
            {
                Id = x.Id,
                DisplayName = x.FullName,
                PersonnelId = x.PersonnelId

            }).ToListAsync();
        }

        public Task<WorkerVewiModel> GetWorker(Guid workerId)
        {
            return TableNoTracking.Where(t => t.Id == workerId)
                .Select(w => new WorkerVewiModel
                {
                    Shift = w.ShiftStatus,
                    ShiftOverTimeSalary = w.ShiftOverTimeSalary,
                    ShiftSalary = w.ShiftSalary,
                    StartDate = w.StartDate,
                    Status = w.Status,
                    OverTimeSalary = w.OverTimeSalary,
                    AccountNumber = w.AccountNumber,
                    Address = w.Address,
                    DayInMonth = w.DayInMonth,
                    Description = w.Description,
                    FullName = w.FullName,
                    PersonnelId = w.PersonnelId,
                    WorkerStatus = w.Status.ToDisplay(DisplayProperty.Name),
                    NationalCode = w.NationalCode,
                    EndDate = w.EndDate,
                    Id = w.Id,
                    InsurancePremium = w.InsurancePremium,
                    JobTitle = w.JobTitle,
                    Mobile = w.Mobile
                }).FirstOrDefaultAsync();
        }


        public Task<List<WorkerVewiModel>> GetWorkers(string fullName, string jobTitle, string nationalCode, Status status, int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            return TableNoTracking
                .Where(x => string.IsNullOrEmpty(fullName) || x.FullName.Contains(fullName))
                .Where(x => string.IsNullOrEmpty(jobTitle) || x.JobTitle.Contains(jobTitle))
                .Where(x => string.IsNullOrEmpty(nationalCode) || x.NationalCode.Contains(nationalCode))
                .Where(x => status == Status.All || x.Status == status)
                .Select(t => new WorkerVewiModel
                {
                    Id = t.Id,
                    JobTitle = t.JobTitle,
                    WorkerStatus = t.Status.ToDisplay(DisplayProperty.Name),
                    Status = t.Status,
                    Shift = t.ShiftStatus,
                    ShiftSalary = t.ShiftSalary,
                    ShiftOverTimeSalary = t.ShiftOverTimeSalary,
                    StartDate = t.StartDate,
                    AccountNumber = t.AccountNumber,
                    Address = t.Address,
                    Description = t.Description,
                    EndDate = t.EndDate,
                    Mobile = t.Mobile,
                    PersonnelId = t.PersonnelId,
                    FullName = t.FullName,
                    NationalCode = t.NationalCode,
                    Salary = t.Salary,
                    OverTimeSalary = t.OverTimeSalary,
                    DayInMonth = t.DayInMonth,
                    InsurancePremium = t.InsurancePremium,
                })
                .Skip(pageNum * NeAccountingConstants.PageCount)
                .Take(NeAccountingConstants.PageCount)
                .ToListAsync();
        }

        public async Task<(string error, bool isSuccess)> Create(
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
            byte dayInMonth)
        {
            var worker = await TableNoTracking.FirstOrDefaultAsync(t => t.NationalCode == natinalCode || t.PersonnelId == personalId);

            if (worker != null)
            {
                if (worker.NationalCode == natinalCode)
                    return new($" کارگر {worker.FullName} با این کد ملی در پایگاه داده موجود می‌باشد!!!", false);

                if (worker.PersonnelId == personalId)
                    return new($" کارگر {worker.FullName} با این کد پرسنلی در پایگاه داده موجود می‌باشد!!!", false);
            }

            try
            {
                await Entities.AddAsync(new Personel(
                   fullName,
                   natinalCode,
                   mobile,
                   address,
                   personalId,
                   accountNumber,
                   description,
                   jobTitle,
                   startDate,
                   shift,
                   salary,
                   overtimeSalary,
                   shiftSalary,
                   shiftOvertimeSalary,
                   insurancePremium,
                   dayInMonth));
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(68t46923)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> Update(
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
            byte dayInMonth)
        {
            var worker = await TableNoTracking.FirstOrDefaultAsync(t => t.Id == id);


            if (worker == null)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            if (natinalCode != worker.NationalCode)
            {
                var w = await TableNoTracking.FirstOrDefaultAsync(t => t.Id != id && t.NationalCode == natinalCode);
                if (w != null)
                {
                    return new("کارگر دیگری با این کد ملی موجود می‌باشد!!!!", false);
                }
            }

            try
            {
                worker.SetNationalCode(natinalCode);
                worker.SetAccountNumber(accountNumber);
                worker.SetDesc(description);
                worker.SetFullName(fullName);
                worker.SetMobile(mobile);
                worker.SetAddress(address);
                worker.SetJobTitele(jobTitle);
                worker.PersonnelId = personalId;
                worker.Status = status;
                worker.StartDate = startDate;
                worker.ShiftStatus = shift;
                worker.Salary = salary;
                worker.OverTimeSalary = overtimeSalary;
                worker.ShiftSalary = shiftSalary;
                worker.ShiftOverTimeSalary = shiftOvertimeSalary;
                worker.InsurancePremium = insurancePremium;
                worker.DayInMonth = dayInMonth;

                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(18t43973)!!!", false);
            }
            return new(string.Empty, true);
        }
        #endregion

        #region Salary
        public async Task<(bool isSuccess, SalaryWorkerViewModel item)> GetSalaryDetailBySalaryId(Guid workerId, int salaryId, byte persianMonth, int persianYear)
        {
            var salarise = await (from w in DbContext.Set<Personel>()
                                                 .AsNoTracking()
                                                 .Where(t => t.Id == workerId)

                                  join s in DbContext.Set<Salary>()
                                                          .Where(t => t.Id == salaryId)
                                                          on w.Id equals s.WorkerId

                                  join a in DbContext.Set<FinancialAid>()
                                  .Where(c => c.PersianMonth == persianMonth && c.PersianYear == persianYear)
                                                          on w.Id equals a.WorkerId into ai
                                  from aid in ai.DefaultIfEmpty()

                                  select new SalaryWorkerViewModel()
                                  {
                                      WorkerName = w.FullName,
                                      PersonelId = w.PersonnelId,
                                      ShiftStatus = w.ShiftStatus,
                                      Insurance = s.Insurance,
                                      FinancialAid = aid.AmountOf == null ? 0 : aid.AmountOf,
                                      AmountOf = s.AmountOf,
                                      OverTime = s.OverTime,
                                      SubmitMonth = s.PersianMonth,
                                      SubmitYear = s.PersianYear,
                                      ChildAllowance = s.ChildAllowance,
                                      Description = s.Description,
                                      LeftOver = s.LeftOver,
                                      LoanInstallment = s.LoanInstallment,
                                      OtherAdditions = s.OtherAdditions,
                                      OtherDeductions = s.OtherDeductions,
                                      RightHousingAndFood = s.RightHousingAndFood,
                                      Tax = s.Tax,
                                      Error = string.Empty,
                                      Success = true,
                                  }).ToListAsync();

            if (salarise.FirstOrDefault() == null)
            {
                return (false, new SalaryWorkerViewModel());
            }
            long amountOf = (long)salarise.Sum(x => x.FinancialAid);
            salarise.First().FinancialAid = amountOf;
            return (true, salarise.First());
        }

        public async Task<(bool isSuccess, SalaryWorkerViewModel item)> GetSalaryDetailBySalaryId(int salaryId, byte persianMonth, int persianYear)
        {
            var salarise = await (from s in DbContext.Set<Salary>()
                                                 .AsNoTracking()
                                                 .Include(t => t.Worker)
                                                 .Where(t => t.Id == salaryId)

                                  join f in DbContext.Set<Function>()
                                  .Where(t => t.PersianYear == persianYear)
                                  .Where(t => t.PersianMonth == persianMonth)
                                                          on s.WorkerId equals f.WorkerId


                                  join a in DbContext.Set<FinancialAid>()
                                  .Where(c => c.PersianMonth == persianMonth && c.PersianYear == persianYear)
                                                          on s.WorkerId equals a.WorkerId into ai
                                  from aid in ai.DefaultIfEmpty()

                                  select new SalaryWorkerViewModel()
                                  {
                                      WorkerName = s.Worker.FullName,
                                      PersonelId = s.Worker.PersonnelId,
                                      ShiftStatus = s.Worker.ShiftStatus,
                                      Insurance = s.Insurance,
                                      FinancialAid = aid.AmountOf == null ? 0 : aid.AmountOf,
                                      AmountOf = s.AmountOf,
                                      OverTime = s.OverTime,
                                      SubmitMonth = s.PersianMonth,
                                      SubmitYear = s.PersianYear,
                                      ChildAllowance = s.ChildAllowance,
                                      Description = s.Description,
                                      FunctionNum = f.AmountOf,
                                      OverTimeNum = f.AmountOfOverTime,
                                      LeftOver = s.LeftOver,
                                      LoanInstallment = s.LoanInstallment,
                                      OtherAdditions = s.OtherAdditions,
                                      OtherDeductions = s.OtherDeductions,
                                      RightHousingAndFood = s.RightHousingAndFood,
                                      Tax = s.Tax,
                                      Error = string.Empty,
                                      Success = true,
                                  }).ToListAsync();

            if (salarise.FirstOrDefault() == null)
            {
                return (false, new SalaryWorkerViewModel());
            }
            long amountOf = salarise.Sum(x => x.FinancialAid);
            salarise.First().FinancialAid = amountOf;
            return (true, salarise.First());
        }

        public async Task<PagedResulViewModel<SalaryViewModel>> GetSalaryList(Guid? workerId,
            byte? startMonth,
            int? startYear,
            int? endMonth,
            int? endYear,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
            await EnsureConnectionOpenAsync();
            var parameters = new[] // sqlINput
        {
            new SqlParameter(nameof(workerId), workerId == null ? DBNull.Value : workerId),
            new SqlParameter(nameof(startMonth), startMonth == null ? DBNull.Value : startMonth),
            new SqlParameter(nameof(startYear), startYear == null ? DBNull.Value : startYear),
            new SqlParameter(nameof(endMonth), endMonth == null ? DBNull.Value : endMonth),
            new SqlParameter(nameof(endYear), endYear == null ? DBNull.Value : endYear),
            new SqlParameter("skipCount", (pageNum-1) * pageCount),
            new SqlParameter("maxResultCount",pageCount)
        };

            int totalCount = 0;
            List<SalaryViewModel> rows = new();
            using (var command = CreateCommand(SqlStoredProcedureConstants.GetSalaryList, CommandType.StoredProcedure, parameters))
            {
                using var dataReader = await command.ExecuteReaderAsync();
                while (await dataReader.ReadAsync()) //Sql OutPut
                {
                    SalaryViewModel row = new();
                    row.FullName = (string)dataReader[nameof(row.FullName)];
                    row.AmountOf = ((long)dataReader[nameof(row.AmountOf)]).ToString("N0");
                    row.LeftOver = ((long)dataReader[nameof(row.LeftOver)]).ToString("N0");
                    row.OverTime = ((long)dataReader[nameof(row.OverTime)]).ToString("N0");
                    row.TotalDebt = ((long)dataReader[nameof(row.TotalDebt)]).ToString("N0");
                    row.PersianMonth = (byte)dataReader[nameof(row.PersianMonth)];
                    row.PersianYear = (int)dataReader[nameof(row.PersianYear)];
                    row.Details = new SalaryDetails()
                    {
                        Id = (int)dataReader[nameof(row.Details.Id)],
                        WorkerId = (Guid)dataReader[nameof(row.Details.WorkerId)],
                        PersianMonth = (byte)dataReader[nameof(row.PersianMonth)],
                        PersianYear = (int)dataReader[nameof(row.PersianYear)]
                    };
                    totalCount = ((int)dataReader[("TotalRecord")]);
                    rows.Add(row);
                }
            }
            return new PagedResulViewModel<SalaryViewModel>(totalCount, pageCount, pageNum, rows);
        }

        public async Task<long> GetTotalSalary()
        {
            return await DbContext.Set<Salary>().SumAsync(t => t.AmountOf + t.ChildAllowance + t.RightHousingAndFood + t.OverTime + t.OtherAdditions);
        }


        public async Task<(string error, bool isSuccess)> AddSalary(Guid workerId,
            byte persianMonth,
            int persianYear,
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
            string? description)
        {
            var worker = await Entities
                .Include(s => s.Salaries)
                .Include(s => s.Functions.Where(t =>
                t.PersianYear == persianYear && t.PersianMonth == persianMonth))
                .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            if (worker.Functions.Count == 0)
            {
                return new(" برای ماه مورد نظر هیچ کارکردی ثبت نشده!!!", false);
            }

            if (worker.Salaries.Any(t =>
            t.PersianMonth == persianMonth && t.PersianYear == persianYear))
            {
                return new(" برای این پرسنل در این ماه فیش حقوقی صادر شده !!!", false);
            }

            try
            {
                worker.AddSalary(new Salary(
                        persianYear,
                        persianMonth,
                        amountOf,
                        financialAid,
                        overTime,
                        tax,
                        childAllowance,
                        rightHousingAndFood,
                        insurance,
                        loanInstallment,
                        otherAdditions,
                        otherDeductions,
                        leftOver,
                        description));

                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(70t46993)!!!", false);
            }
            return new(string.Empty, true);

        }

        public async Task<(string error, bool isSuccess)> UpdateSalary(Guid workerId,
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
            string? description)
        {
            var worker = await Entities
                .Include(s => s.Salaries)
                .Include(s => s.Functions.Where(t => t.PersianYear == persianYear && t.PersianMonth == persianMonth))
                .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            if (worker.Salaries.FirstOrDefault(t => t.Id == salaryId) == null)
                return new("فیش مورد نظر یافت نشد!!!!", false);

            if (worker.Salaries.Any(t =>
                t.Id != salaryId && t.PersianMonth == persianMonth && t.PersianYear == persianYear))
            {
                return new(" برای این پرسنل در این ماه فیش حقوقی صادر شده !!!", false);
            }

            if (worker.Functions.Count == 0)
            {
                return new(" برای ماه مورد نظر هیچ کارکردی ثبت نشده!!!", false);
            }
            var salary = worker.Salaries.First(s => s.Id == salaryId);

            try
            {
                salary.SetDesc(description);
                salary.PersianMonth = persianMonth;
                salary.PersianYear = persianYear;
                salary.Insurance = insurance;
                salary.OtherDeductions = otherDeductions;
                salary.ChildAllowance = childAllowance;
                salary.AmountOf = amountOf;
                salary.LeftOver = leftOver;
                salary.OtherAdditions = otherAdditions;
                salary.RightHousingAndFood = rightHousingAndFood;
                salary.FinancialAid = financialAid;
                salary.OverTime = overTime;
                salary.Tax = tax;
                salary.LoanInstallment = loanInstallment;

                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(60t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> DeleteSalary(Guid workerId, int salaryId)
        {
            var worker = await Table
               .Include(t => t.Salaries.Where(s => s.Id == salaryId))
               .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null || worker.Salaries.Count == 0)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            var salary = worker.Salaries.First();

            try
            {
                worker.Salaries.Remove(salary);
                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return new(" خطا در اتصال به پایگاه داده code(50t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<SalaryWorkerViewModel> GetSalaryDetailByWorkerId(Guid workerId, byte persianMonth,
            int persianYear, int? salaryId = null)
        {
            long aid = 0;
            long ssalary = 0;
            long overtime = 0;

            var worker = await TableNoTracking
                .Include(t => t.Salaries.Where(s => s.PersianYear == persianYear && s.PersianMonth == persianMonth))
                .Include(t => t.Functions.Where(f => f.PersianYear == persianYear && f.PersianMonth == persianMonth))
                .Include(t => t.Aids.Where(a => a.PersianYear == persianYear && a.PersianMonth == persianMonth))
                .FirstAsync(t => t.Id == workerId);

            var salary = worker.Salaries.FirstOrDefault();
            if (worker.Salaries.Count != 0 && salary != null && salary.Id != salaryId)
            {
                return new SalaryWorkerViewModel() { Error = "برای پرسنل مورد نظر در این ماه فیش حقوقی صادر شده!!!", Success = false };
            }

            if (worker.Functions.Count == 0)
                return new SalaryWorkerViewModel() { Error = "برای پرسنل مورد نظر کارکردی در این ماه ثبت نشده !!!", Success = false };


            if (worker.Aids.Count != 0)
            {
                aid = (long)worker.Aids.Sum(t => t.AmountOf);
            }

            var func = worker.Functions.First();


            if (worker.ShiftStatus == Shift.ByMounth)
            {
                if (func.AmountOf == worker.DayInMonth)
                {
                    ssalary = worker.Salary;
                }
                else
                {
                    ssalary = func.AmountOf * (worker.Salary / worker.DayInMonth);
                }
                overtime = func.AmountOfOverTime * worker.OverTimeSalary;

            }
            else
            {
                ssalary = func.AmountOf * worker.ShiftSalary;
                overtime = func.AmountOfOverTime * worker.ShiftOverTimeSalary;

            }

            var details = new SalaryWorkerViewModel()
            {
                PersonelId = worker.PersonnelId,
                ShiftStatus = worker.ShiftStatus,
                Insurance = worker.InsurancePremium,
                FinancialAid = aid,
                AmountOf = ssalary,
                OverTime = overtime,
                Error = string.Empty,
                Success = true,
            };

            if (salary != null)
            {
                details.RightHousingAndFood = salary.RightHousingAndFood;
                details.ChildAllowance = salary.ChildAllowance;
                details.OtherAdditions = salary.OtherAdditions;
                details.Insurance = salary.Insurance;
                details.Tax = salary.Tax;
                details.LoanInstallment = salary.LoanInstallment;
                details.OtherDeductions = salary.OtherDeductions;
                details.Description = salary.Description;
            }
            return details;
        }
        #endregion

        #region Function
        public async Task<(string error, bool isSuccess)> AddFunctuion(
            Guid workerId,
            int persianYear,
            byte persianMonth,
            byte amountOf,
            byte amountOfOverTime,
            string? description)
        {
            var worker = await Entities.Include(w => w.Functions.Where(c => c.PersianMonth == persianMonth && c.PersianYear == persianYear))
                .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null)
            {
                return new("کارگر مورد نظر یافت نشد!!!", false);
            }
            if (worker.Functions.Count != 0)
            {
                return new("برای این پرسنل در این ماه کارکرد ثبت شده !!!", false);
            }

            try
            {
                worker.AddFunction(new Function(persianMonth, persianYear, amountOf, amountOfOverTime, description));
                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(80t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateFunc(
           Guid workerId,
           int persianYear,
           byte persianMonth,
           int funcId,
           byte amountOf,
           byte overTime,
           string? description)
        {

            var worker = await Entities
                .Include(c => c.Salaries)
                .Include(c => c.Functions)
                .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            var func = worker.Functions.FirstOrDefault(t => t.Id == funcId);

            if (func == null)
                return new("کارکرد مورد نظر یافت نشد!!!!", false);

            if (worker.Functions.FirstOrDefault(t => t.Id != funcId && t.PersianYear == persianYear && t.PersianMonth == persianMonth) != null)
                return new("برای کارگر مورد نظر در این ماه کارکرد ثبت شده!!!!", false);

            if (worker.Salaries.FirstOrDefault(t => t.PersianYear == persianYear && t.PersianMonth == persianMonth) != null)
                return new("برای ماه مورد نظر فیش حقوقی صادر شده!!!\n در صورت نیاز به ویرایش ابتدا فیش حقوقی ماه مرتبط را حذف کرده و مجددا تلاش نمایید.", false);

            try
            {
                func.SetDesc(description);
                func.PersianMonth = persianMonth;
                func.PersianYear = persianYear;
                func.AmountOf = amountOf;
                func.AmountOfOverTime = overTime;
                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(90t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> DeleteFunc(Guid workerId,
            int persianYear,
            byte persianMonth,
            int funcId)
        {
            var worker = await Entities
               .Include(t => t.Salaries.Where(s => s.PersianMonth == persianMonth && s.PersianYear == persianYear))
               .Include(c => c.Functions.Where(c => c.Id == funcId))
               .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null || worker.Functions.Count == 0)
            {
                return new("کارگر مورد نظر یافت نشد!!!!", false);
            }

            var func = worker.Functions.FirstOrDefault(t => t.Id == funcId);

            if (func == null)
                return new("کارکرد مورد نظر یافت نشد!!!!", false);

            if (worker.Salaries.FirstOrDefault(t => t.PersianYear == persianYear && t.PersianMonth == persianMonth) != null)
                return new("برای ماه مورد نظر فیش حقوقی صادر شده!!!\n در صورت نیاز به ویرایش ابتدا فیش حقوقی ماه مرتبط را حذف کرده و مجددا تلاش نمایید.", false);

            try
            {
                worker.Functions.Remove(func);
                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(01t46993)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<PagedResulViewModel<FunctionViewModel>> GetFunctionList(Guid? workerId,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
			var list = await (from worker in DbContext.Set<Personel>()
                                                         .AsNoTracking()
                                                         .Where(t => workerId == null || t.Id == workerId)

                          join func in DbContext.Set<Function>()
                                                  on worker.Id equals func.WorkerId

                          select new FunctionViewModel()
                          {
                              Name = worker.FullName,
                              Amountof = func.AmountOf,
                              OverTime = func.AmountOfOverTime,
                              Description = func.Description,
                              PersonelId = worker.PersonnelId,
                              PersianMonth = func.PersianMonth,
                              PersianYear = func.PersianYear,
                              Details = new FucntionDetails() { Id = func.Id, WorkerId = worker.Id }
                          })
                      .OrderByDescending(c => c.PersianYear)
                      .ThenByDescending(c => c.PersianMonth)
                      .ToListAsync();
			int row = 1;
			var totalCount = list.Count;
			list = list.Skip(--pageNum * pageCount).Take(pageCount).ToList();
			return new PagedResulViewModel<FunctionViewModel>(totalCount, pageCount, pageNum, list);
		}

        #endregion

        #region Aid
        public async Task<(string error, bool isSuccess)> AddAid(
            DateTime submitDate,
            Guid workerId,
            int persianYear,
            byte persianMonth,
            long amountOf,
            string? description)
        {
            var worker = await Entities
               .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null)
            {
                return new("کارگر مورد نظر یافت نشد!!!", false);
            }

            try
            {
                worker.AddAid(new FinancialAid(submitDate, persianMonth, persianYear, amountOf, description));
                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(68t96423)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> UpdateAid(
            DateTime subDate,
            Guid workerId,
            int persianYear,
            byte persianMonth,
            int aidId,
            long amount,
            string? description)
        {
            var worker = await Entities
                .Include(c => c.Salaries.Where(t => t.PersianMonth == persianMonth && t.PersianYear == persianYear))
                .Include(c => c.Aids.Where(c => c.Id == aidId))
                .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            var aid = worker.Aids.FirstOrDefault(t => t.Id == aidId);

            if (aid == null)
                return new("مساعده مورد نظر یافت نشد!!!!", false);

            if (worker.Salaries.Count != 0)
                return new("برای ماه مورد نظر فیش حقوقی صادر شده!!!\n در صورت نیاز به ویرایش ابتدا فیش حقوقی ماه مرتبط را حذف کرده و مجددا تلاش نمایید.", false);

            try
            {
                aid.SetDesc(description);
                aid.SubmitDate = subDate;
                aid.PersianMonth = persianMonth;
                aid.PersianYear = persianYear;
                aid.AmountOf = amount;
                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(68t43423)!!!", false);
            }
            return new(string.Empty, true);
        }

        public async Task<(string error, bool isSuccess)> DeleteAid(Guid workerId,
            int persianYear,
            byte persianMonth,
            int aidId)
        {
            var worker = await Entities
                .Include(c => c.Salaries.Where(t => t.PersianMonth == persianMonth && t.PersianYear == persianYear))
                .Include(c => c.Aids.Where(c => c.Id == aidId))
                .FirstOrDefaultAsync(t => t.Id == workerId);

            if (worker == null)
                return new("کارگر مورد نظر یافت نشد!!!!", false);

            var aid = worker.Aids.FirstOrDefault(t => t.Id == aidId);

            if (aid == null)
                return new("مساعده مورد نظر یافت نشد!!!!", false);

            if (worker.Salaries.Count != 0)
                return new("برای ماه مورد نظر فیش حقوقی صادر شده!!!\n در صورت نیاز به ویرایش ابتدا فیش حقوقی ماه مرتبط را حذف کرده و مجددا تلاش نمایید.", false);

            try
            {
                worker.Aids.Remove(aid);
                Entities.Update(worker);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException aex)
                {
                    return new(aex.Message, false);
                }
                return new(" خطا در اتصال به پایگاه داده code(31t46948)!!!", false);

            }
            return new(string.Empty, true);
        }

        public async Task<PagedResulViewModel<AidViewModel>> GetAidList(Guid? workerId, int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount)
        {
			var list = await (from worker in DbContext.Set<Personel>()
                                               .AsNoTracking()
                                               .Where(t => workerId == null || t.Id == workerId)


                          join aid in DbContext.Set<FinancialAid>()
                                                  on worker.Id equals aid.WorkerId

                          select new AidViewModel()
                          {
                              Name = worker.FullName,
                              AmountPrice = aid.AmountOf,
                              Description = aid.Description,
                              PersonelId = worker.PersonnelId,
                              Price = aid.AmountOf.ToString("N0"),
                              SubmitDate = aid.SubmitDate,
                              Details = new AidDetails() { Id = aid.Id, WorkerId = worker.Id }
                          })
                          .OrderByDescending(t => t.SubmitDate)
                          .ToListAsync();
			int row = 1;
			var totalCount = list.Count;
			list = list.Skip(--pageNum * pageCount).Take(pageCount).ToList();
			return new PagedResulViewModel<AidViewModel>(totalCount, pageCount, pageNum, list);

		}
        #endregion
    }
}
