using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    /// <inheritdoc />
    public partial class AddCheque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cheque",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    PayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReciverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumetnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubmitStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    TransferdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecivedOrPayDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Due_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cheque_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Serial = table.Column<long>(type: "bigint", nullable: false),
                    Accunt_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bank_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bank_Branch = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cheque_Owner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_Cheque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cheque_Customer_PayerId",
                        column: x => x.PayerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cheque_Customer_ReciverId",
                        column: x => x.ReciverId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Cheque_Document_DocumetnId",
                        column: x => x.DocumetnId,
                        principalTable: "Document",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_DocumetnId",
                table: "Cheque",
                column: "DocumetnId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_Id",
                table: "Cheque",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_PayerId",
                table: "Cheque",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cheque_ReciverId",
                table: "Cheque",
                column: "ReciverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cheque");
        }
    }
}
