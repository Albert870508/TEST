using Microsoft.EntityFrameworkCore.Migrations;

namespace TEST.EntityFrameworkCore.Migrations
{
    public partial class addAnswerRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Examinations",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Examinations");
        }
    }
}
