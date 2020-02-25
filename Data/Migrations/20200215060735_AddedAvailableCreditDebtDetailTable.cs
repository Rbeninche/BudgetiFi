using Microsoft.EntityFrameworkCore.Migrations;

namespace BudgetiFi.Data.Migrations
{
    public partial class AddedAvailableCreditDebtDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "AvailableCredit",
                table: "DebtDetails",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableCredit",
                table: "DebtDetails");
        }
    }
}
