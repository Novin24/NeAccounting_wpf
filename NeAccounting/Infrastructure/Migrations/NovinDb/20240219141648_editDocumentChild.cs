using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class editDocumentChild : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Type",
                table: "Document",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "Document",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "PayType",
                table: "Document",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Document_DocumentId",
                table: "Document",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Document_DocumentId",
                table: "Document",
                column: "DocumentId",
                principalTable: "Document",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Document_DocumentId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_DocumentId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "PayType",
                table: "Document");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Document",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }
    }
}
