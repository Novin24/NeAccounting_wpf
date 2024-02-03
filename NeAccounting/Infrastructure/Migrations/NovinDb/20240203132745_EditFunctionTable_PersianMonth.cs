using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class EditFunctionTable_PersianMonth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersanMonth",
                table: "Function",
                newName: "PersianMonth");

            migrationBuilder.RenameIndex(
                name: "IX_Function_PersanMonth",
                table: "Function",
                newName: "IX_Function_PersianMonth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersianMonth",
                table: "Function",
                newName: "PersanMonth");

            migrationBuilder.RenameIndex(
                name: "IX_Function_PersianMonth",
                table: "Function",
                newName: "IX_Function_PersanMonth");
        }
    }
}
