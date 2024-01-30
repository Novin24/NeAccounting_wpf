using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class updatefuncAndAids : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubmitDate",
                table: "FinancialAid",
                newName: "PayDate");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAid_Worker_WorkerId",
                table: "FinancialAid",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Function_Worker_WorkerId",
                table: "Function",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAid_Worker_WorkerId",
                table: "FinancialAid");

            migrationBuilder.DropForeignKey(
                name: "FK_Function_Worker_WorkerId",
                table: "Function");

            migrationBuilder.RenameColumn(
                name: "PayDate",
                table: "FinancialAid",
                newName: "SubmitDate");
        }
    }
}
