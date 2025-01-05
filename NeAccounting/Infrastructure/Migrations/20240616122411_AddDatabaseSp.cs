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
CREATE OR ALTER PROCEDURE dbo.AddDatabase @DbName NVARCHAR(128),
@dFileName NVARCHAR(MAX),
@dLogFileName NVARCHAR(MAX)
AS
BEGIN
  SET NOCOUNT ON;

  BEGIN TRY

    DECLARE @SQLCreate NVARCHAR(MAX)
           ,@SQLAlter NVARCHAR(MAX)
           ,@SQLStoreProcedure NVARCHAR(MAX)
           ,@ErrorText NVARCHAR(MAX)
           ,@Success BIT
    SET @SQLCreate = N'
      CREATE DATABASE ' + QUOTENAME(@DbName) + N' ON PRIMARY
      (NAME = ' + QUOTENAME(@DbName) + N',
       FILENAME = ''' + @dFileName + N''',
       SIZE = 8192KB,
       MAXSIZE = UNLIMITED,
       FILEGROWTH = 65536KB)
      LOG ON (NAME = ' + QUOTENAME(@DbName + '_Log') + N',
              FILENAME = ''' + @dLogFileName + N''',
              SIZE = 8192KB,
              MAXSIZE = UNLIMITED,
              FILEGROWTH = 65536KB) 
              
  ALTER DATABASE ' + QUOTENAME(@DbName) + N'
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
   
  
  ALTER DATABASE ' + QUOTENAME(@DbName) + N'
    SET ENABLE_BROKER
   
  
  ALTER DATABASE ' + QUOTENAME(@DbName) + N'
    SET ALLOW_SNAPSHOT_ISOLATION OFF
   
  
  ALTER DATABASE ' + QUOTENAME(@DbName) + N'
    SET FILESTREAM (NON_TRANSACTED_ACCESS = OFF)
   
  
  EXECUTE sp_configure ''show advanced options'', 1;
   
  RECONFIGURE;
   
  EXECUTE sp_configure ''nested triggers'', 1;
   
  RECONFIGURE;';
    EXEC sp_executesql @SQLCreate;




    SET @SQLAlter = N'
  ALTER DATABASE ' + QUOTENAME(@DbName) + N'
    SET QUERY_STORE = OFF
  USE ' + QUOTENAME(@DbName) + N'
  ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
  ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
  ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
  ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
  ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
  ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
  ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
  ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
  USE ' + QUOTENAME(@DbName) + N'
  IF DB_NAME() <> N''' + @DbName + N''' SET NOEXEC ON
  
  
  CREATE TABLE dbo.Units (
    Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
    Name nvarchar(30) NOT NULL,
    Descrip nvarchar(100) NULL,
    IsActive bit NOT NULL,
    CreationTime datetime2 NOT NULL,
    CreatorId uniqueidentifier NOT NULL,
    LastModificationTime datetime2 NULL,
    LastModifireId uniqueidentifier NULL,
    DeletionTime datetime2 NULL,
    DeleterId uniqueidentifier NULL,
    IsDeleted bit NOT NULL,
    IdNumber INT IDENTITY,
    CONSTRAINT PK_Units PRIMARY KEY CLUSTERED (Id)
  )
  ON [PRIMARY]   
  
  CREATE INDEX IX_Units_Id
    ON dbo.Units (Id)
    ON [PRIMARY]
   
  
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
   
  
  CREATE INDEX IX_SellRemittance_DocumentId
    ON dbo.SellRemittance (DocumentId)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_SellRemittance_MaterialId
    ON dbo.SellRemittance (MaterialId)
    ON [PRIMARY]
   
  
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
   
  
  CREATE INDEX IX_Salary_PersianMonth
    ON dbo.Salary (PersianMonth)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_Salary_PersianYear
    ON dbo.Salary (PersianYear)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_Salary_WorkerId
    ON dbo.Salary (WorkerId)
    ON [PRIMARY]
   
  
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
    MiniEntity FLOAT NOT NULL DEFAULT (0.0000000000000000e+000)
    CONSTRAINT PK_Pun PRIMARY KEY CLUSTERED (Id)
  )
  ON [PRIMARY]
   
  
  CREATE INDEX IX_Pun_Id
    ON dbo.Pun (Id)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_Pun_UnitId
    ON dbo.Pun (UnitId)
    ON [PRIMARY]
   
  
  CREATE TABLE dbo.Personel (
    Id uniqueidentifier NOT NULL DEFAULT (newsequentialid()),
    FullName nvarchar(50) NOT NULL,
    NationalCode nvarchar(12) NOT NULL,
    Mobile nvarchar(20) NOT NULL,
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
  
  CREATE INDEX IX_Personel_PersonnelId
    ON dbo.Personel (PersonnelId)
    ON [PRIMARY]
   
  
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
   
  
  CREATE INDEX IX_Function_PersianMonth
    ON dbo.[Function] (PersianMonth)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_Function_PersianYear
    ON dbo.[Function] (PersianYear)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_Function_WorkerId
    ON dbo.[Function] (WorkerId)
    ON [PRIMARY]
   
  
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
   
  
  CREATE INDEX IX_FinancialAid_PersianMonth
    ON dbo.FinancialAid (PersianMonth)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_FinancialAid_PersianYear
    ON dbo.FinancialAid (PersianYear)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_FinancialAid_WorkerId
    ON dbo.FinancialAid (WorkerId)
    ON [PRIMARY]
   
  
  SET QUOTED_IDENTIFIER, ANSI_NULLS ON
  
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
   
  
  CREATE INDEX IX_Expense_Id
    ON dbo.Expense (Id)
    ON [PRIMARY]
   
  
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
   
  
  CREATE INDEX IX_Document_CustomerId
    ON dbo.Document (CustomerId)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_Document_DocumentId
    ON dbo.Document (DocumentId)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_Document_Id
    ON dbo.Document (Id)
    ON [PRIMARY]
   
  
  CREATE INDEX IX_Document_Serial
    ON dbo.Document (Serial)
    ON [PRIMARY]
   
  
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
   
  
  CREATE INDEX IX_Customer_Id
    ON dbo.Customer (Id)
    ON [PRIMARY]
    
    
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
  Cheque_Series NVARCHAR(100) NULL,
  SiadyNumber NVARCHAR(100) NULL,
  CONSTRAINT PK_Cheque PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
 

CREATE INDEX IX_Cheque_DocumetnId
  ON dbo.Cheque (DocumetnId)
  ON [PRIMARY]
 

CREATE INDEX IX_Cheque_Id
  ON dbo.Cheque (Id)
  ON [PRIMARY]
 

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
 

CREATE INDEX IX_BuyRemittance_DocumentId
  ON dbo.BuyRemittance (DocumentId)
  ON [PRIMARY]
 

CREATE INDEX IX_BuyRemittance_MaterialId
  ON dbo.BuyRemittance (MaterialId)
  ON [PRIMARY]
 
CREATE TABLE dbo.__EFMigrationsHistory (
  MigrationId nvarchar(150) NOT NULL,
  ProductVersion nvarchar(32) NOT NULL,
  CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY CLUSTERED (MigrationId)
)
ON [PRIMARY]


INSERT dbo.__EFMigrationsHistory VALUES (N''20240428082541_initDatabase'', N''8.0.3'')



SET IDENTITY_INSERT dbo.Customer ON
INSERT dbo.Customer(Id, Name, CusId, Mobile, TotalCredit, ChequeCredit, CashCredit, PromissoryNote, NationalCode, Address, Buyer, Seller, Type, HaveChequeGuarantee, HaveCashCredit, HavePromissoryNote, IsActive, CreationTime, CreatorId, LastModificationTime, LastModifireId, DeletionTime, DeleterId, IsDeleted) VALUES (''00000000-0000-0000-0000-000000000000'', N''صندوق'', 3806, N''0'', 0, 0, 0, 0, N''0'', N''-'', 0, 0, 1, 0, 0, 0, 1, ''1900-01-01 00:00:00.0000000'', ''7193ab51-9d88-4559-8b5e-4028bb1b1a49'', NULL, NULL, NULL, NULL, 0)
SET IDENTITY_INSERT dbo.Customer OFF


USE ' + QUOTENAME(@DbName) + N'


IF DB_NAME() <> N''' + @DbName + N'''
  SET NOEXEC ON


ALTER TABLE dbo.Salary
ADD CONSTRAINT FK_Salary_Personel_WorkerId FOREIGN KEY (WorkerId) REFERENCES dbo.Personel (Id) ON DELETE CASCADE


ALTER TABLE dbo.Pun
ADD CONSTRAINT FK_Pun_Units_UnitId FOREIGN KEY (UnitId) REFERENCES dbo.Units (Id) ON DELETE CASCADE


ALTER TABLE dbo.[Function]
ADD CONSTRAINT FK_Function_Personel_WorkerId FOREIGN KEY (WorkerId) REFERENCES dbo.Personel (Id) ON DELETE CASCADE


ALTER TABLE dbo.FinancialAid
ADD CONSTRAINT FK_FinancialAid_Personel_WorkerId FOREIGN KEY (WorkerId) REFERENCES dbo.Personel (Id) ON DELETE CASCADE


ALTER TABLE dbo.Document
ADD CONSTRAINT FK_Document_Document_DocumentId FOREIGN KEY (DocumentId) REFERENCES dbo.Document (Id)


ALTER TABLE dbo.SellRemittance
ADD CONSTRAINT FK_SellRemittance_Document_DocumentId FOREIGN KEY (DocumentId) REFERENCES dbo.Document (Id) ON DELETE CASCADE


ALTER TABLE dbo.SellRemittance
ADD CONSTRAINT FK_SellRemittance_Pun_MaterialId FOREIGN KEY (MaterialId) REFERENCES dbo.Pun (Id) ON DELETE CASCADE


ALTER TABLE dbo.Cheque
ADD CONSTRAINT FK_Cheque_Document_DocumetnId FOREIGN KEY (DocumetnId) REFERENCES dbo.Document (Id) ON DELETE CASCADE


ALTER TABLE dbo.BuyRemittance
ADD CONSTRAINT FK_BuyRemittance_Document_DocumentId FOREIGN KEY (DocumentId) REFERENCES dbo.Document (Id) ON DELETE CASCADE


ALTER TABLE dbo.BuyRemittance
ADD CONSTRAINT FK_BuyRemittance_Pun_MaterialId FOREIGN KEY (MaterialId) REFERENCES dbo.Pun (Id) ON DELETE CASCADE

SET NOEXEC OFF'
    EXEC sp_executesql @SQLAlter;




    SET @SQLStoreProcedure = N'
USE ' + QUOTENAME(@DbName) + N';
EXEC (''
CREATE OR ALTER PROCEDURE dbo.GetSalaryList
    @workerId UNIQUEIDENTIFIER = NULL,
    @StartMonth TINYINT = NULL,
    @StartYear INT = NULL,
    @EndMonth INT = NULL,
    @EndYear INT = NULL,
    @SkipCount INT,
    @MaxResultCount INT
AS
BEGIN
    SET NOCOUNT ON;
    
    WITH D_CTE AS (
        SELECT
            w.Id AS WorkerId,
            s.Id AS Id,
            w.FullName,
            s.AmountOf,
            s.PersianYear,
            s.LeftOver,
            s.PersianMonth,
            s.OverTime,
            ISNULL((
                SELECT SUM(fa.AmountOf)
                FROM FinancialAid fa
                WHERE fa.IsDeleted = 0
                AND fa.WorkerId = s.WorkerId
                AND fa.PersianMonth = s.PersianMonth
                AND fa.PersianYear = s.PersianYear
            ), 0) + s.LoanInstallment + s.OtherDeductions + s.Tax + s.Insurance AS TotalDebt
        FROM Personel w
        JOIN Salary s ON w.Id = s.WorkerId
        WHERE s.IsDeleted = 0
        AND (@StartMonth IS NULL OR s.PersianMonth >= @StartMonth)
        AND (@EndMonth IS NULL OR s.PersianMonth <= @EndMonth)
        AND (@workerId IS NULL OR w.Id = @workerId)
        AND (@StartYear IS NULL OR s.PersianYear >= @StartYear)
        AND (@EndYear IS NULL OR s.PersianYear <= @EndYear)
        AND w.IsDeleted = 0
    ),
    Count_CTE AS (
        SELECT COUNT(*) AS TotalRecord FROM D_CTE
    )
    SELECT *
    FROM D_CTE, Count_CTE
    ORDER BY D_CTE.PersianYear DESC, D_CTE.PersianMonth DESC
    OFFSET (@SkipCount) ROWS FETCH NEXT @MaxResultCount ROWS ONLY;
END;
'');';
    EXEC sys.sp_executesql @SQLStoreProcedure

    SET @Success = 1; -- Set success to true
    SET @ErrorText = '';
    SELECT
      @Success IsSuccess
     ,@ErrorText ErrorMessage
  END TRY
  BEGIN CATCH
    -- Roll back the transaction in case of any error       
    SET @Success = 0; -- Set success to false
    SET @ErrorText = ERROR_MESSAGE();

    SELECT
      @Success IsSuccess
     ,@ErrorText ErrorMessage
  END CATCH;
END
GO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
