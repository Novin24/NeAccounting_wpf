using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class RemChequePayerAndRecivr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheque_Customer_CustomerId",
                table: "Cheque");

            migrationBuilder.DropForeignKey(
                name: "FK_Cheque_Customer_CustomerId1",
                table: "Cheque");

            migrationBuilder.DropIndex(
                name: "IX_Cheque_CustomerId",
                table: "Cheque");

            migrationBuilder.DropIndex(
                name: "IX_Cheque_CustomerId1",
                table: "Cheque");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Cheque");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Cheque");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Cheque",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId1",
                table: "Cheque",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_CustomerId",
                table: "Cheque",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_CustomerId1",
                table: "Cheque",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheque_Customer_CustomerId",
                table: "Cheque",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheque_Customer_CustomerId1",
                table: "Cheque",
                column: "CustomerId1",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
