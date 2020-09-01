using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace backend.Migrations
{
    public partial class AddModeratorRolesToAutoModPlugin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModeratorRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GuildId = table.Column<decimal>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    AutoModPluginId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeratorRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModeratorRole_AutoModPlugin_AutoModPluginId",
                        column: x => x.AutoModPluginId,
                        principalTable: "AutoModPlugin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModeratorRole_AutoModPluginId",
                table: "ModeratorRole",
                column: "AutoModPluginId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModeratorRole");
        }
    }
}
