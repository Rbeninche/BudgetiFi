using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BudgetiFi.Data.Migrations
{
    public partial class CreateDebtDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DebtDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    CreditLImit = table.Column<double>(nullable: false),
                    Balance = table.Column<double>(nullable: false),
                    MinimumPayment = table.Column<double>(nullable: false),
                    DebtCategoryId = table.Column<int>(nullable: false),
                    CreditorCategoryId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DebtDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DebtDetails_CreditorCategories_CreditorCategoryId",
                        column: x => x.CreditorCategoryId,
                        principalTable: "CreditorCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DebtDetails_DebtCategories_DebtCategoryId",
                        column: x => x.DebtCategoryId,
                        principalTable: "DebtCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DebtDetails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DebtDetails_CreditorCategoryId",
                table: "DebtDetails",
                column: "CreditorCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DebtDetails_DebtCategoryId",
                table: "DebtDetails",
                column: "DebtCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_DebtDetails_UserId",
                table: "DebtDetails",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DebtDetails");
        }
    }
}
