using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class editMaterial_lastSellPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastPrice",
                table: "Material",
                newName: "LastSellPrice");

            migrationBuilder.AddColumn<long>(
                name: "LastBuyPrice",
                table: "Material",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastBuyPrice",
                table: "Material");

            migrationBuilder.RenameColumn(
                name: "LastSellPrice",
                table: "Material",
                newName: "LastPrice");
        }
    }
}
