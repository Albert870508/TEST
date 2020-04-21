using Microsoft.EntityFrameworkCore.Migrations;

namespace TEST.EntityFrameworkCore.Migrations
{
    public partial class AddDepartmentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionTypeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ValidateCode",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentId",
                table: "Users",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "TotalScore",
                table: "Scores",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Scores",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ExaminationId",
                table: "Scores",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "ExaminationId",
                table: "Scores");

            migrationBuilder.AddColumn<long>(
                name: "QuestionTypeId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ValidateCode",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TotalScore",
                table: "Scores",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
