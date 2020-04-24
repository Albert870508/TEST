using Microsoft.EntityFrameworkCore.Migrations;

namespace TEST.EntityFrameworkCore.Migrations
{
    public partial class adddayorweek : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DayOrWeek",
                table: "Scores",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOrWeek",
                table: "Scores");
        }
    }
}
