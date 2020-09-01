using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class RefactorCommandActionRoleIdType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RoleId",
                table: "CommandAction",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RoleId",
                table: "CommandAction",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
