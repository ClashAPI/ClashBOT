using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class RefactorModeratorRoleModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "RoleId",
                table: "ModeratorRole",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "ModeratorRole",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
