using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class addIndexToSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Salary_PersianMonth",
                table: "Salary",
                column: "PersianMonth");

            migrationBuilder.CreateIndex(
                name: "IX_Salary_PersianYear",
                table: "Salary",
                column: "PersianYear");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Salary_PersianMonth",
                table: "Salary");

            migrationBuilder.DropIndex(
                name: "IX_Salary_PersianYear",
                table: "Salary");
        }
    }
}
