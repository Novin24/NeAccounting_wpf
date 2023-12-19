using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class edit_Worker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "DayInMonth",
                table: "Worker",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<long>(
                name: "InsurancePremium",
                table: "Worker",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "OverTimeSalary",
                table: "Worker",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Salary",
                table: "Worker",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayInMonth",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "InsurancePremium",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "OverTimeSalary",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "Worker");
        }
    }
}
