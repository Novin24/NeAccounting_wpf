using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class editCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Etebar",
                table: "Customer",
                newName: "TotalCredit");

            migrationBuilder.AddColumn<long>(
                name: "CashCredit",
                table: "Customer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ChequeCredit",
                table: "Customer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "HaveCashCredit",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HaveChequeGuarantee",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HavePromissoryNote",
                table: "Customer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "PromissoryNote",
                table: "Customer",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashCredit",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "ChequeCredit",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "HaveCashCredit",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "HaveChequeGuarantee",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "HavePromissoryNote",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "PromissoryNote",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Customer");

            migrationBuilder.RenameColumn(
                name: "TotalCredit",
                table: "Customer",
                newName: "Etebar");
        }
    }
}
