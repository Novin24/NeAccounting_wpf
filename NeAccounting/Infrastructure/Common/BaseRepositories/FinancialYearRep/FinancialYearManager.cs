using Domain.BaseDomain.FinancialYears;
using DomainShared.Constants;
using DomainShared.Extension;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.PagedResul;
using Infrastructure.EntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NeApplication.IBaseRepositories;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Infrastructure.BaseRepositories
{
    public class FinancialYearManager(BaseDomainDbContext context) : BaseRepository<FinancialYear>(context), IFinancialYearManager
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

        #region CreateDatabase
        public async Task<(bool isSuccess, string error)> CreateNewDatabase(string databaseName,
           string dFileName,
           string dLogFileName)
        {
            await EnsureConnectionOpenAsync();
            var parameters = new[] // sqlINput
        {
            new SqlParameter("DbName",databaseName),
            new SqlParameter(nameof(dFileName), dFileName),
            new SqlParameter(nameof(dLogFileName), dLogFileName)
        };
            bool isSuccess = false;
            string error = string.Empty;
            using (var command = CreateCommand(SqlStoredProcedureConstants.AddDatabase, CommandType.StoredProcedure, parameters))
            {
                using var dataReader = await command.ExecuteReaderAsync();


                while (await dataReader.ReadAsync()) //Sql OutPut
                {
                    isSuccess = ((bool)dataReader[("IsSuccess")]);
                    error = ((string)dataReader[("ErrorMessage")]);
                    //    SalaryViewModel row = new();
                    //    row.FullName = (string)dataReader[nameof(row.FullName)];
                    //    row.AmountOf = ((long)dataReader[nameof(row.AmountOf)]).ToString("N0");
                    //    row.LeftOver = ((long)dataReader[nameof(row.LeftOver)]).ToString("N0");
                    //    row.OverTime = ((long)dataReader[nameof(row.OverTime)]).ToString("N0");
                    //    row.TotalDebt = ((long)dataReader[nameof(row.TotalDebt)]).ToString("N0");
                    //    row.PersianMonth = (byte)dataReader[nameof(row.PersianMonth)];
                    //    row.PersianYear = (int)dataReader[nameof(row.PersianYear)];
                    //    row.Details = new SalaryDetails()
                    //    {
                    //        Id = (int)dataReader[nameof(row.Details.Id)],
                    //        WorkerId = (Guid)dataReader[nameof(row.Details.WorkerId)],
                    //        PersianMonth = (byte)dataReader[nameof(row.PersianMonth)],
                    //        PersianYear = (int)dataReader[nameof(row.PersianYear)]
                    //    };
                    //    totalCount = ((int)dataReader[("TotalRecord")]);
                    //    rows.Add(row);
                }
            }
            return new(isSuccess, error);
        }
        #endregion

        public async Task<(bool isSuccess, string databaseName, bool isCurrent)> GetActiveYear()
        {
            var t = await TableNoTracking.FirstOrDefaultAsync(t => t.IsActive);
            if (t == null)
            {
                return new(false, string.Empty, false);
            }
            return new(true, t.DataBaseName, t.IsCurrent);
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

        public async Task<bool> CheckNameExist(string name)
        {
            return await TableNoTracking.AnyAsync(t => t.Name == name);
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
