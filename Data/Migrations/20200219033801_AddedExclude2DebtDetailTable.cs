using Microsoft.EntityFrameworkCore.Migrations;

namespace BudgetiFi.Data.Migrations
{
    public partial class AddedExclude2DebtDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exclude",
                table: "DebtDetails");

            migrationBuilder.AddColumn<bool>(
                name: "Ignore",
                table: "DebtDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ignore",
                table: "DebtDetails");

            migrationBuilder.AddColumn<bool>(
                name: "Exclude",
                table: "DebtDetails",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
