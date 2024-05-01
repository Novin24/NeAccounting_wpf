using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class addSalaryListSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE dbo.GetSalaryList @workerId INT = NULL,
                @StartMonth tinyint = NULL,
                @StartYear INT = NULL,
                @EndMonth INT = NULL,
                @EndYear INT = NULL,
                @SkipCount INT,
                @MaxResultCount INT
                AS
                BEGIN
                  SET NOCOUNT ON;
                  WITH D_CTE
                  AS
                  (SELECT
                      w.Id WorkerId
                     ,s.Id Id
                     ,w.FullName
                     ,s.AmountOf
                     ,s.PersianYear
                     ,s.LeftOver
                     ,s.PersianMonth
                     ,s.OverTime
                     ,ISNULL((SELECT
                          SUM(fa.AmountOf)
                        FROM FinancialAid fa
                        WHERE fa.IsDeleted = 0
                        AND fa.WorkerId = s.WorkerId
                        AND fa.PersianMonth = s.PersianMonth
                        AND fa.PersianYear = s.PersianYear)
                      , 0) + s.LoanInstallment + s.OtherDeductions + s.Tax + s.Insurance TotalDebt
                    FROM Worker w
                    JOIN Salary s
                      ON w.Id = s.WorkerId
                    WHERE s.IsDeleted = 0
                    AND (@StartMonth IS NULL
                    OR s.PersianMonth >= @StartMonth)
                    AND (@EndMonth IS NULL
                    OR s.PersianMonth <= @EndMonth)
                    AND (@workerId IS NULL
                    OR w.Id = @workerId)
                    AND (@StartYear IS NULL
                    OR s.PersianYear >= @StartYear)
                    AND (@EndYear IS NULL
                    OR s.PersianYear <= @EndYear)
                    AND w.IsDeleted = 0),
                  Count_CTE
                  AS
                  (SELECT
                      COUNT(*) [TotalRecord]
                    FROM D_CTE)
                  SELECT
                    *
                  FROM D_CTE
                      ,Count_CTE
                  ORDER BY D_CTE.PersianYear DESC, D_CTE.PersianMonth DESC
                  OFFSET (@SkipCount) ROWS FETCH NEXT @MaxResultCount ROWS ONLY
                END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
