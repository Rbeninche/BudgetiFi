using Microsoft.EntityFrameworkCore.Migrations;

namespace BudgetiFi.Data.Migrations
{
    public partial class ChangeDebtDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreditLImit",
                table: "DebtDetails",
                newName: "CreditLimit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreditLimit",
                table: "DebtDetails",
                newName: "CreditLImit");
        }
    }
}
