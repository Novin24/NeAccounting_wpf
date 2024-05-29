using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class editPunProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "PunProduct");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "PunProduct");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "PunProduct");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "PunProduct");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PunProduct");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PunProduct");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "PunProduct");

            migrationBuilder.DropColumn(
                name: "LastModifireId",
                table: "PunProduct");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "PunProduct",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "PunProduct",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "PunProduct",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "PunProduct",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PunProduct",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWSEQUENTIALID()");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PunProduct",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "PunProduct",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifireId",
                table: "PunProduct",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
