using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class addManyToMay_pun : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRawmaterial",
                table: "Pun",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "PunProduct",
                columns: table => new
                {
                    ProductionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RawMaterialId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WastePercentage = table.Column<byte>(type: "tinyint", nullable: false),
                    UsagePercentage = table.Column<byte>(type: "tinyint", nullable: false),
                    Ratio = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
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
                    table.PrimaryKey("PK_PunProduct", x => new { x.ProductionId, x.RawMaterialId });
                    table.ForeignKey(
                        name: "FK_PunProduct_Pun_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "Pun",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PunProduct_Pun_RawMaterialId",
                        column: x => x.RawMaterialId,
                        principalTable: "Pun",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PunProduct_RawMaterialId",
                table: "PunProduct",
                column: "RawMaterialId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PunProduct");

            migrationBuilder.DropColumn(
                name: "IsRawmaterial",
                table: "Pun");
        }
    }
}
