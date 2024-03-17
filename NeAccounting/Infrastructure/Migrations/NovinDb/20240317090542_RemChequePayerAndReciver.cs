using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class RemChequePayerAndReciver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheque_Customer_PayerId",
                table: "Cheque");

            migrationBuilder.DropForeignKey(
                name: "FK_Cheque_Customer_ReciverId",
                table: "Cheque");

            migrationBuilder.DropIndex(
                name: "IX_Cheque_PayerId",
                table: "Cheque");

            migrationBuilder.DropIndex(
                name: "IX_Cheque_ReciverId",
                table: "Cheque");

            migrationBuilder.DropColumn(
                name: "PayerId",
                table: "Cheque");

            migrationBuilder.DropColumn(
                name: "RecivedOrPayDate",
                table: "Cheque");

            migrationBuilder.DropColumn(
                name: "ReciverId",
                table: "Cheque");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "PayerId",
                table: "Cheque",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "RecivedOrPayDate",
                table: "Cheque",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ReciverId",
                table: "Cheque",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_PayerId",
                table: "Cheque",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_ReciverId",
                table: "Cheque",
                column: "ReciverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheque_Customer_PayerId",
                table: "Cheque",
                column: "PayerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cheque_Customer_ReciverId",
                table: "Cheque",
                column: "ReciverId",
                principalTable: "Customer",
                principalColumn: "Id");
        }
    }
}
