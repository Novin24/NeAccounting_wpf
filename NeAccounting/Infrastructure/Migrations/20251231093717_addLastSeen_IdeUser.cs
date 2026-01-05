using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addLastSeen_IdeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NtionalCode",
                table: "IdentityUser",
                newName: "NationalCode");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSeen",
                table: "IdentityUser",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastSeen",
                table: "IdentityUser");

            migrationBuilder.RenameColumn(
                name: "NationalCode",
                table: "IdentityUser",
                newName: "NtionalCode");
        }
    }
}
