using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace backend.Migrations
{
    public partial class AddTemporaryBansToGuilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Messages",
                table: "CommandAction");

            migrationBuilder.DropColumn(
                name: "RoleIds",
                table: "CommandAction");

            migrationBuilder.CreateTable(
                name: "TemporaryBan",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<decimal>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryBan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporaryBan_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryBan_GuildId",
                table: "TemporaryBan",
                column: "GuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemporaryBan");

            migrationBuilder.AddColumn<string>(
                name: "Messages",
                table: "CommandAction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoleIds",
                table: "CommandAction",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
