using Microsoft.EntityFrameworkCore.Migrations;

namespace School.Migrations
{
    public partial class AddFieUpodNColum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "fileUploadName",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fileUploadName",
                table: "Files");
        }
    }
}
