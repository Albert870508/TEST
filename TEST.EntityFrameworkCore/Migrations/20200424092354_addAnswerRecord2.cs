using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TEST.EntityFrameworkCore.Migrations
{
    public partial class addAnswerRecord2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerRecords",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    QuestionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerRecords", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerRecords");
        }
    }
}
