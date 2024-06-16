using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabaseSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER PROCEDURE dbo.AddDatabase @DbName NVARCHAR(100),
@FilePath NVARCHAR(100),
@logFileName NVARCHAR(100),
@LogFilePath NVARCHAR(100)
AS
BEGIN
  DECLARE @SQL NVARCHAR(MAX)
         ,@Params NVARCHAR(MAX);
  SET @SQL = N' 
SET DATEFORMAT ymd
SET ARITHABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
SET NUMERIC_ROUNDABORT, IMPLICIT_TRANSACTIONS, XACT_ABORT OFF
GO

IF DB_NAME() <> N''BaseDomain'' SET NOEXEC ON

CREATE DATABASE QUOTENAME(@Name)
ON PRIMARY(
    NAME = @dName,
    FILENAME =  @dFileName,
    SIZE = 8192KB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 65536KB
)
LOG ON(
    NAME = @dLogName,
    FILENAME = @dLogFileName,
    SIZE = 8192KB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 65536KB
)
GO

ALTER DATABASE QUOTENAME(@Name)
  SET
    ANSI_NULL_DEFAULT OFF,
    ANSI_NULLS OFF,
    ANSI_PADDING OFF,
    ANSI_WARNINGS OFF,
    ARITHABORT OFF,
    AUTO_CLOSE ON,
    AUTO_CREATE_STATISTICS ON,
    AUTO_SHRINK OFF,
    AUTO_UPDATE_STATISTICS ON,
    AUTO_UPDATE_STATISTICS_ASYNC OFF,
    COMPATIBILITY_LEVEL = 150,
    CONCAT_NULL_YIELDS_NULL OFF,
    CURSOR_CLOSE_ON_COMMIT OFF,
    CURSOR_DEFAULT GLOBAL,
    DATE_CORRELATION_OPTIMIZATION OFF,
    DB_CHAINING OFF,
    HONOR_BROKER_PRIORITY OFF,
    MULTI_USER,
    NUMERIC_ROUNDABORT OFF,
    PAGE_VERIFY CHECKSUM,
    PARAMETERIZATION SIMPLE,
    QUOTED_IDENTIFIER OFF,
    READ_COMMITTED_SNAPSHOT ON,
    RECOVERY SIMPLE,
    RECURSIVE_TRIGGERS OFF,
    TRANSFORM_NOISE_WORDS = OFF,
    TRUSTWORTHY OFF
    WITH ROLLBACK IMMEDIATE
GO

ALTER DATABASE QUOTENAME(@Name)
  SET ENABLE_BROKER
GO

ALTER DATABASE QUOTENAME(@Name)
  SET ALLOW_SNAPSHOT_ISOLATION OFF
GO

ALTER DATABASE QUOTENAME(@Name)
  SET FILESTREAM (NON_TRANSACTED_ACCESS = OFF)
GO

EXECUTE sp_configure ''show advanced options'', 1;
GO
RECONFIGURE;
GO
EXECUTE sp_configure ''nested triggers'', 1;
GO
RECONFIGURE;
GO

ALTER DATABASE QUOTENAME(@Name)
  SET QUERY_STORE = OFF
GO

USE QUOTENAME(@Name)
GO

ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO

ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO

ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO

USE QUOTENAME(@Name)
GO

IF DB_NAME() <> @dName SET NOEXEC ON
GO

CREATE TABLE dbo.Units (
  Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
  Name nvarchar(30) NOT NULL,
  Descrip nvarchar(max) NULL,
  IsActive bit NOT NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Units PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO


CREATE INDEX IX_Units_Id
  ON dbo.Units (Id)
  ON [PRIMARY]
GO


CREATE TABLE dbo.SellRemittance (
  Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
  MaterialId uniqueidentifier NOT NULL,
  DocumentId uniqueidentifier NOT NULL,
  Price bigint NOT NULL,
  Description nvarchar(100) NULL,
  SubmitDate datetime2 NOT NULL,
  TotalPrice bigint NOT NULL,
  AmountOf float NOT NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_SellRemittance PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO


CREATE INDEX IX_SellRemittance_DocumentId
  ON dbo.SellRemittance (DocumentId)
  ON [PRIMARY]
GO


CREATE INDEX IX_SellRemittance_MaterialId
  ON dbo.SellRemittance (MaterialId)
  ON [PRIMARY]
GO


CREATE TABLE dbo.Salary (
  Id int IDENTITY,
  WorkerId uniqueidentifier NOT NULL,
  PersianMonth tinyint NOT NULL,
  PersianYear int NOT NULL,
  AmountOf bigint NOT NULL,
  FinancialAid bigint NOT NULL,
  OverTime bigint NOT NULL,
  Tax bigint NOT NULL,
  ChildAllowance bigint NOT NULL,
  RightHousingAndFood bigint NOT NULL,
  Insurance bigint NOT NULL,
  LoanInstallment bigint NOT NULL,
  OtherAdditions bigint NOT NULL,
  OtherDeductions bigint NOT NULL,
  LeftOver bigint NOT NULL,
  Description nvarchar(200) NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Salary PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO


CREATE INDEX IX_Salary_PersianMonth
  ON dbo.Salary (PersianMonth)
  ON [PRIMARY]
GO


CREATE INDEX IX_Salary_PersianYear
  ON dbo.Salary (PersianYear)
  ON [PRIMARY]
GO


CREATE INDEX IX_Salary_WorkerId
  ON dbo.Salary (WorkerId)
  ON [PRIMARY]
GO


CREATE TABLE dbo.Pun (
  Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
  UnitId uniqueidentifier NOT NULL,
  Name nvarchar(100) NOT NULL,
  Serial nvarchar(50) NOT NULL,
  Entity float NOT NULL,
  LastSellPrice bigint NOT NULL,
  LastBuyPrice bigint NOT NULL,
  IsManufacturedGoods bit NOT NULL,
  PhysicalAddress nvarchar(100) NOT NULL,
  IsActive bit NOT NULL DEFAULT (CONVERT([bit],(1))),
  IsService bit NOT NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Pun PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO


CREATE INDEX IX_Pun_Id
  ON dbo.Pun (Id)
  ON [PRIMARY]
GO


CREATE INDEX IX_Pun_UnitId
  ON dbo.Pun (UnitId)
  ON [PRIMARY]
GO


CREATE TABLE dbo.Personel (
  Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
  FullName nvarchar(50) NOT NULL,
  NationalCode nvarchar(10) NULL,
  Mobile nvarchar(max) NOT NULL,
  Address nvarchar(150) NULL,
  StartDate datetime2 NOT NULL,
  EndDate datetime2 NULL,
  PersonnelId int NOT NULL,
  AccountNumber nvarchar(26) NOT NULL,
  Description nvarchar(200) NULL,
  Status tinyint NOT NULL,
  ShiftStatus tinyint NOT NULL,
  JobTitle nvarchar(50) NOT NULL,
  Salary bigint NOT NULL,
  OverTimeSalary bigint NOT NULL,
  ShiftSalary bigint NOT NULL,
  ShiftOverTimeSalary bigint NOT NULL,
  InsurancePremium bigint NOT NULL,
  DayInMonth tinyint NOT NULL,
  IsActive bit NOT NULL DEFAULT (CONVERT([bit],(1))),
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Personel PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO


CREATE INDEX IX_Personel_PersonnelId
  ON dbo.Personel (PersonnelId)
  ON [PRIMARY]
GO


CREATE TABLE dbo.[Function] (
  Id int IDENTITY,
  WorkerId uniqueidentifier NOT NULL,
  PersianYear int NOT NULL,
  PersianMonth tinyint NOT NULL,
  AmountOf tinyint NOT NULL,
  AmountOfOverTime tinyint NOT NULL,
  Description nvarchar(200) NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Function PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO


CREATE INDEX IX_Function_PersianMonth
  ON dbo.[Function] (PersianMonth)
  ON [PRIMARY]
GO


CREATE INDEX IX_Function_PersianYear
  ON dbo.[Function] (PersianYear)
  ON [PRIMARY]
GO
CREATE INDEX IX_Function_WorkerId
  ON dbo.[Function] (WorkerId)
  ON [PRIMARY]
GO

CREATE TABLE dbo.FinancialAid (
  Id int IDENTITY,
  WorkerId uniqueidentifier NOT NULL,
  SubmitDate datetime2 NOT NULL,
  PersianYear int NOT NULL,
  PersianMonth tinyint NOT NULL,
  AmountOf bigint NOT NULL,
  Description nvarchar(200) NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_FinancialAid PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO

CREATE INDEX IX_FinancialAid_PersianMonth
  ON dbo.FinancialAid (PersianMonth)
  ON [PRIMARY]
GO

CREATE INDEX IX_FinancialAid_PersianYear
  ON dbo.FinancialAid (PersianYear)
  ON [PRIMARY]
GO

CREATE INDEX IX_FinancialAid_WorkerId
  ON dbo.FinancialAid (WorkerId)
  ON [PRIMARY]
GO

SET QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
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
END
GO
CREATE TABLE dbo.Expense (
  Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
  SubmitDate datetime2 NOT NULL,
  Expensetype nvarchar(150) NOT NULL,
  Amount bigint NOT NULL,
  PayType tinyint NOT NULL,
  Receiver nvarchar(100) NULL,
  Description nvarchar(250) NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Expense PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO
CREATE INDEX IX_Expense_Id
  ON dbo.Expense (Id)
  ON [PRIMARY]
GO
CREATE TABLE dbo.Document (
  Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
  CustomerId uniqueidentifier NOT NULL,
  DocumentId uniqueidentifier NULL,
  Price bigint NOT NULL,
  Description nvarchar(150) NULL,
  SubmitDate datetime2 NOT NULL,
  PayType tinyint NOT NULL,
  Type tinyint NOT NULL,
  IsReceived bit NOT NULL,
  Commission tinyint NULL,
  Serial bigint IDENTITY (4732, 1),
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Document PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO
CREATE INDEX IX_Document_CustomerId
  ON dbo.Document (CustomerId)
  ON [PRIMARY]
GO
CREATE INDEX IX_Document_DocumentId
  ON dbo.Document (DocumentId)
  ON [PRIMARY]
GO
CREATE INDEX IX_Document_Id
  ON dbo.Document (Id)
  ON [PRIMARY]
GO
CREATE INDEX IX_Document_Serial
  ON dbo.Document (Serial)
  ON [PRIMARY]
GO
CREATE TABLE dbo.Customer (
  Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
  Name nvarchar(50) NOT NULL,
  CusId bigint IDENTITY (397, 1),
  Mobile nvarchar(20) NOT NULL,
  TotalCredit bigint NOT NULL,
  ChequeCredit bigint NOT NULL,
  CashCredit bigint NOT NULL,
  PromissoryNote bigint NOT NULL,
  NationalCode nvarchar(10) NOT NULL,
  Address nvarchar(150) NOT NULL,
  Buyer bit NOT NULL,
  Seller bit NOT NULL,
  Type tinyint NOT NULL,
  HaveChequeGuarantee bit NOT NULL,
  HaveCashCredit bit NOT NULL,
  HavePromissoryNote bit NOT NULL,
  IsActive bit NOT NULL DEFAULT (CONVERT([bit],(1))),
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Customer PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO
CREATE INDEX IX_Customer_Id
  ON dbo.Customer (Id)
  ON [PRIMARY]
GO
CREATE TABLE dbo.Cheque (
  Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
  DocumetnId uniqueidentifier NOT NULL,
  Payer uniqueidentifier NOT NULL,
  Reciver uniqueidentifier NOT NULL,
  SubmitStatus tinyint NOT NULL,
  Status tinyint NOT NULL,
  TransferdDate datetime2 NULL,
  Due_Date datetime2 NULL,
  Cheque_Number nvarchar(100) NOT NULL,
  Serial bigint IDENTITY (987, 1),
  Accunt_Number nvarchar(100) NULL,
  Bank_Name nvarchar(50) NOT NULL,
  Bank_Branch nvarchar(50) NULL,
  Cheque_Owner nvarchar(50) NOT NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Cheque PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO
CREATE INDEX IX_Cheque_DocumetnId
  ON dbo.Cheque (DocumetnId)
  ON [PRIMARY]
GO
CREATE INDEX IX_Cheque_Id
  ON dbo.Cheque (Id)
  ON [PRIMARY]
GO
CREATE TABLE dbo.BuyRemittance (
  Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
  MaterialId uniqueidentifier NOT NULL,
  DocumentId uniqueidentifier NOT NULL,
  Price bigint NOT NULL,
  Description nvarchar(100) NULL,
  SubmitDate datetime2 NOT NULL,
  TotalPrice bigint NOT NULL,
  AmountOf float NOT NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_BuyRemittance PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO
CREATE INDEX IX_BuyRemittance_DocumentId
  ON dbo.BuyRemittance (DocumentId)
  ON [PRIMARY]
GO
CREATE INDEX IX_BuyRemittance_MaterialId
  ON dbo.BuyRemittance (MaterialId)
  ON [PRIMARY]
GO
CREATE TABLE dbo.Bank (
  Id int IDENTITY,
  Bank_ID int NOT NULL,
  Bank_Name nvarchar(max) NOT NULL,
  Bank_Branch nvarchar(max) NOT NULL,
  Account_num nvarchar(max) NOT NULL,
  CreationTime datetime2 NOT NULL,
  CreatorId uniqueidentifier NOT NULL,
  LastModificationTime datetime2 NULL,
  LastModifireId uniqueidentifier NULL,
  DeletionTime datetime2 NULL,
  DeleterId uniqueidentifier NULL,
  IsDeleted bit NOT NULL,
  CONSTRAINT PK_Bank PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE dbo.__EFMigrationsHistory (
  MigrationId nvarchar(150) NOT NULL,
  ProductVersion nvarchar(32) NOT NULL,
  CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY CLUSTERED (MigrationId)
)
ON [PRIMARY]
GO
INSERT dbo.__EFMigrationsHistory VALUES (N''20240428082541_initDatabase'', N''8.0.3'')
GO

SET IDENTITY_INSERT dbo.Customer ON
GO
INSERT dbo.Customer (Id, Name, Mobile, TotalCredit, NationalCode, Address, Buyer, Seller, CreationTime, CreatorId, LastModificationTime, LastModifireId, DeletionTime, DeleterId, IsDeleted, CashCredit, ChequeCredit, HaveCashCredit, HaveChequeGuarantee, HavePromissoryNote, PromissoryNote, Type, CusId, IsActive)
  VALUES (''00000000-0000-0000-0000-000000000000'', N''صندوق'', N''0'', 0, N''0'', N''-'', 0,0, CONVERT(DATETIME2, ''1900-01-01 00:00:00.0000000'', 121), ''7193ab51-9d88-4559-8b5e-4028bb1b1a49'', NULL, NULL, NULL, NULL, 0, 0, 0, 0, 0, 0, 0, 1, 111, 1)
GO
SET IDENTITY_INSERT dbo.Customer OFF

USE QUOTENAME(@Name)
GO

IF DB_NAME() <> @dName SET NOEXEC ON
GO

ALTER TABLE dbo.Salary
  ADD CONSTRAINT FK_Salary_Personel_WorkerId FOREIGN KEY (WorkerId) REFERENCES dbo.Personel (Id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Pun
  ADD CONSTRAINT FK_Pun_Units_UnitId FOREIGN KEY (UnitId) REFERENCES dbo.Units (Id) ON DELETE CASCADE
GO

ALTER TABLE dbo.[Function]
  ADD CONSTRAINT FK_Function_Personel_WorkerId FOREIGN KEY (WorkerId) REFERENCES dbo.Personel (Id) ON DELETE CASCADE
GO

ALTER TABLE dbo.FinancialAid
  ADD CONSTRAINT FK_FinancialAid_Personel_WorkerId FOREIGN KEY (WorkerId) REFERENCES dbo.Personel (Id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Document
  ADD CONSTRAINT FK_Document_Document_DocumentId FOREIGN KEY (DocumentId) REFERENCES dbo.Document (Id)
GO

ALTER TABLE dbo.SellRemittance
  ADD CONSTRAINT FK_SellRemittance_Document_DocumentId FOREIGN KEY (DocumentId) REFERENCES dbo.Document (Id) ON DELETE CASCADE
GO

ALTER TABLE dbo.SellRemittance
  ADD CONSTRAINT FK_SellRemittance_Pun_MaterialId FOREIGN KEY (MaterialId) REFERENCES dbo.Pun (Id) ON DELETE CASCADE
GO

ALTER TABLE dbo.Cheque
  ADD CONSTRAINT FK_Cheque_Document_DocumetnId FOREIGN KEY (DocumetnId) REFERENCES dbo.Document (Id) ON DELETE CASCADE
GO
ALTER TABLE dbo.BuyRemittance
  ADD CONSTRAINT FK_BuyRemittance_Document_DocumentId FOREIGN KEY (DocumentId) REFERENCES dbo.Document (Id) ON DELETE CASCADE
GO

ALTER TABLE dbo.BuyRemittance
  ADD CONSTRAINT FK_BuyRemittance_Pun_MaterialId FOREIGN KEY (MaterialId) REFERENCES dbo.Pun (Id) ON DELETE CASCADE
GO
SET NOEXEC OFF
GO';
  SET @Params = N'@dName varchar(100), @dFileName varchar(MAX), @dLogName varchar(100), @dLogFileName varchar(MAX)';
  EXEC sp_executesql @SQL
                    ,@Params
                    ,@dName = @DbName
                    ,@dFileName = @FilePath
                    ,@dLogName = @logFileName
                    ,@dLogFileName = @LogFilePath;
END
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
