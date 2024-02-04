using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class convertAllPersianMonthToByte : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmitDate",
                table: "Function");

            migrationBuilder.DropColumn(
                name: "PayDate",
                table: "FinancialAid");

            migrationBuilder.RenameColumn(
                name: "PersanMonth",
                table: "FinancialAid",
                newName: "PersianMonth");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAid_PersanMonth",
                table: "FinancialAid",
                newName: "IX_FinancialAid_PersianMonth");

            migrationBuilder.AlterColumn<byte>(
                name: "PersianMonth",
                table: "Salary",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersianMonth",
                table: "FinancialAid",
                newName: "PersanMonth");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAid_PersianMonth",
                table: "FinancialAid",
                newName: "IX_FinancialAid_PersanMonth");

            migrationBuilder.AlterColumn<int>(
                name: "PersianMonth",
                table: "Salary",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmitDate",
                table: "Function",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PayDate",
                table: "FinancialAid",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
