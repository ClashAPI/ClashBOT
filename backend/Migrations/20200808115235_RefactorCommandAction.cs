using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class RefactorCommandAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "CommandAction",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RoleId",
                table: "CommandAction",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Message",
                table: "CommandAction");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "CommandAction");
        }
    }
}
