using Microsoft.EntityFrameworkCore.Migrations;

namespace BudgetiFi.Data.Migrations
{
    public partial class AddedExcludeDebtDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Exclude",
                table: "DebtDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exclude",
                table: "DebtDetails");
        }
    }
}
