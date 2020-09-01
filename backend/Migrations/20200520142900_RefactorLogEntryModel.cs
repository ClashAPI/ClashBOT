using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class RefactorLogEntryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GuildId",
                table: "LogEntries",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "GuildId",
                table: "LogEntries",
                type: "decimal(20,0)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
