using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class editSalaryRelation : Migration
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

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Unit_UnitId",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Salary_Worker_WorkerId",
                table: "Salary");

            migrationBuilder.CreateIndex(
                name: "IX_Material_Id",
                table: "Material",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Unit_UnitId",
                table: "Material",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Salary_Worker_WorkerId",
                table: "Salary",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAid_Salary_SalaryId",
                table: "FinancialAid");

            migrationBuilder.DropForeignKey(
                name: "FK_Function_Salary_SalaryId",
                table: "Function");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Unit_UnitId",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Salary_Worker_WorkerId",
                table: "Salary");

            migrationBuilder.DropIndex(
                name: "IX_Material_Id",
                table: "Material");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAid_Salary_SalaryId",
                table: "FinancialAid",
                column: "SalaryId",
                principalTable: "Salary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Function_Salary_SalaryId",
                table: "Function",
                column: "SalaryId",
                principalTable: "Salary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Unit_UnitId",
                table: "Material",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Salary_Worker_WorkerId",
                table: "Salary",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
