using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class editSalaryStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAid_Salary_SalaryId",
                table: "FinancialAid");

            migrationBuilder.DropForeignKey(
                name: "FK_Function_Salary_SalaryId",
                table: "Function");

            migrationBuilder.RenameColumn(
                name: "SalaryId",
                table: "Function",
                newName: "WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_Function_SalaryId",
                table: "Function",
                newName: "IX_Function_WorkerId");

            migrationBuilder.RenameColumn(
                name: "SalaryId",
                table: "FinancialAid",
                newName: "WorkerId");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAid_SalaryId",
                table: "FinancialAid",
                newName: "IX_FinancialAid_WorkerId");

            migrationBuilder.AddColumn<byte>(
                name: "PersanMonth",
                table: "Function",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "PersianYear",
                table: "Function",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "PersanMonth",
                table: "FinancialAid",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "PersianYear",
                table: "FinancialAid",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Function_PersanMonth",
                table: "Function",
                column: "PersanMonth");

            migrationBuilder.CreateIndex(
                name: "IX_Function_PersianYear",
                table: "Function",
                column: "PersianYear");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAid_PersanMonth",
                table: "FinancialAid",
                column: "PersanMonth");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAid_PersianYear",
                table: "FinancialAid",
                column: "PersianYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Function_PersanMonth",
                table: "Function");

            migrationBuilder.DropIndex(
                name: "IX_Function_PersianYear",
                table: "Function");

            migrationBuilder.DropIndex(
                name: "IX_FinancialAid_PersanMonth",
                table: "FinancialAid");

            migrationBuilder.DropIndex(
                name: "IX_FinancialAid_PersianYear",
                table: "FinancialAid");

            migrationBuilder.DropColumn(
                name: "PersanMonth",
                table: "Function");

            migrationBuilder.DropColumn(
                name: "PersianYear",
                table: "Function");

            migrationBuilder.DropColumn(
                name: "PersanMonth",
                table: "FinancialAid");

            migrationBuilder.DropColumn(
                name: "PersianYear",
                table: "FinancialAid");

            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "Function",
                newName: "SalaryId");

            migrationBuilder.RenameIndex(
                name: "IX_Function_WorkerId",
                table: "Function",
                newName: "IX_Function_SalaryId");

            migrationBuilder.RenameColumn(
                name: "WorkerId",
                table: "FinancialAid",
                newName: "SalaryId");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAid_WorkerId",
                table: "FinancialAid",
                newName: "IX_FinancialAid_SalaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAid_Salary_SalaryId",
                table: "FinancialAid",
                column: "SalaryId",
                principalTable: "Salary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Function_Salary_SalaryId",
                table: "Function",
                column: "SalaryId",
                principalTable: "Salary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
