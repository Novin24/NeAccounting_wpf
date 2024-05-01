using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class initDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bank_ID = table.Column<int>(type: "int", nullable: false),
                    Bank_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bank_Branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account_num = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CusId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "397, 1"),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalCredit = table.Column<long>(type: "bigint", nullable: false),
                    ChequeCredit = table.Column<long>(type: "bigint", nullable: false),
                    CashCredit = table.Column<long>(type: "bigint", nullable: false),
                    PromissoryNote = table.Column<long>(type: "bigint", nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Buyer = table.Column<bool>(type: "bit", nullable: false),
                    Seller = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    HaveChequeGuarantee = table.Column<bool>(type: "bit", nullable: false),
                    HaveCashCredit = table.Column<bool>(type: "bit", nullable: false),
                    HavePromissoryNote = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SubmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayType = table.Column<byte>(type: "tinyint", nullable: false),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    IsReceived = table.Column<bool>(type: "bit", nullable: false),
                    Commission = table.Column<byte>(type: "tinyint", nullable: true),
                    Serial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "4732, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    SubmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expensetype = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Amount = table.Column<long>(type: "bigint", nullable: false),
                    PayType = table.Column<byte>(type: "tinyint", nullable: false),
                    Receiver = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NationalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonnelId = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    ShiftStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Salary = table.Column<long>(type: "bigint", nullable: false),
                    OverTimeSalary = table.Column<long>(type: "bigint", nullable: false),
                    ShiftSalary = table.Column<long>(type: "bigint", nullable: false),
                    ShiftOverTimeSalary = table.Column<long>(type: "bigint", nullable: false),
                    InsurancePremium = table.Column<long>(type: "bigint", nullable: false),
                    DayInMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Descrip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cheque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    DocumetnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Payer = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reciver = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmitStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    TransferdDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Due_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cheque_Number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Serial = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "987, 1"),
                    Accunt_Number = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Bank_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Bank_Branch = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Cheque_Owner = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cheque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cheque_Document_DocumetnId",
                        column: x => x.DocumetnId,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersianYear = table.Column<int>(type: "int", nullable: false),
                    PersianMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    AmountOf = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAid", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAid_Personel_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Personel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Function",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersianYear = table.Column<int>(type: "int", nullable: false),
                    PersianMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    AmountOf = table.Column<byte>(type: "tinyint", nullable: false),
                    AmountOfOverTime = table.Column<byte>(type: "tinyint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Function_Personel_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Personel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Salary",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersianMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    PersianYear = table.Column<int>(type: "int", nullable: false),
                    AmountOf = table.Column<long>(type: "bigint", nullable: false),
                    FinancialAid = table.Column<long>(type: "bigint", nullable: false),
                    OverTime = table.Column<long>(type: "bigint", nullable: false),
                    Tax = table.Column<long>(type: "bigint", nullable: false),
                    ChildAllowance = table.Column<long>(type: "bigint", nullable: false),
                    RightHousingAndFood = table.Column<long>(type: "bigint", nullable: false),
                    Insurance = table.Column<long>(type: "bigint", nullable: false),
                    LoanInstallment = table.Column<long>(type: "bigint", nullable: false),
                    OtherAdditions = table.Column<long>(type: "bigint", nullable: false),
                    OtherDeductions = table.Column<long>(type: "bigint", nullable: false),
                    LeftOver = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Salary_Personel_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Personel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pun",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    UnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Entity = table.Column<double>(type: "float", nullable: false),
                    LastSellPrice = table.Column<long>(type: "bigint", nullable: false),
                    LastBuyPrice = table.Column<long>(type: "bigint", nullable: false),
                    IsManufacturedGoods = table.Column<bool>(type: "bit", nullable: false),
                    PhysicalAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsService = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pun", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pun_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BuyRemittance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<long>(type: "bigint", nullable: false),
                    AmountOf = table.Column<double>(type: "float", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyRemittance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BuyRemittance_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BuyRemittance_Pun_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Pun",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellRemittance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    MaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SubmitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<long>(type: "bigint", nullable: false),
                    AmountOf = table.Column<double>(type: "float", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifireId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellRemittance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellRemittance_Document_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellRemittance_Pun_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Pun",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BuyRemittance_DocumentId",
                table: "BuyRemittance",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyRemittance_MaterialId",
                table: "BuyRemittance",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_DocumetnId",
                table: "Cheque",
                column: "DocumetnId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_Id",
                table: "Cheque",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_Id",
                table: "Customer",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Document_CustomerId",
                table: "Document",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_DocumentId",
                table: "Document",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_Id",
                table: "Document",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Document_Serial",
                table: "Document",
                column: "Serial");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_Id",
                table: "Expense",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAid_PersianMonth",
                table: "FinancialAid",
                column: "PersianMonth");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAid_PersianYear",
                table: "FinancialAid",
                column: "PersianYear");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAid_WorkerId",
                table: "FinancialAid",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_PersianMonth",
                table: "Function",
                column: "PersianMonth");

            migrationBuilder.CreateIndex(
                name: "IX_Function_PersianYear",
                table: "Function",
                column: "PersianYear");

            migrationBuilder.CreateIndex(
                name: "IX_Function_WorkerId",
                table: "Function",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_Personel_PersonnelId",
                table: "Personel",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pun_Id",
                table: "Pun",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pun_UnitId",
                table: "Pun",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_PersianMonth",
                table: "Salary",
                column: "PersianMonth");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_PersianYear",
                table: "Salary",
                column: "PersianYear");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_WorkerId",
                table: "Salary",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_SellRemittance_DocumentId",
                table: "SellRemittance",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_SellRemittance_MaterialId",
                table: "SellRemittance",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Units_Id",
                table: "Units",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "BuyRemittance");

            migrationBuilder.DropTable(
                name: "Cheque");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Expense");

            migrationBuilder.DropTable(
                name: "FinancialAid");

            migrationBuilder.DropTable(
                name: "Function");

            migrationBuilder.DropTable(
                name: "Salary");

            migrationBuilder.DropTable(
                name: "SellRemittance");

            migrationBuilder.DropTable(
                name: "Personel");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Pun");

            migrationBuilder.DropTable(
                name: "Units");
        }
    }
}
