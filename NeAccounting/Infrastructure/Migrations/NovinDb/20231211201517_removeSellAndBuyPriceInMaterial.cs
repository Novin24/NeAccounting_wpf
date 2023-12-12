using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class removeSellAndBuyPriceInMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "SellPrice",
                table: "Material");

            migrationBuilder.AddColumn<bool>(
                name: "IsManufacturedGoods",
                table: "Material",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManufacturedGoods",
                table: "Material");

            migrationBuilder.AddColumn<long>(
                name: "BuyPrice",
                table: "Material",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "SellPrice",
                table: "Material",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
