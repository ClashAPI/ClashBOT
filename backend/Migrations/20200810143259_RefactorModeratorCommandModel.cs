using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace backend.Migrations
{
    public partial class RefactorModeratorCommandModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Command_AutoModPlugin_AutoModPluginId",
                table: "Command");

            migrationBuilder.DropIndex(
                name: "IX_Command_AutoModPluginId",
                table: "Command");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Command");

            migrationBuilder.DropColumn(
                name: "AutoModPluginId",
                table: "Command");

            migrationBuilder.CreateTable(
                name: "ModeratorCommand",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CommandCall = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Response = table.Column<string>(nullable: true),
                    Prefix = table.Column<string>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    AutoModPluginId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeratorCommand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModeratorCommand_AutoModPlugin_AutoModPluginId",
                        column: x => x.AutoModPluginId,
                        principalTable: "AutoModPlugin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModeratorCommand_AutoModPluginId",
                table: "ModeratorCommand",
                column: "AutoModPluginId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModeratorCommand");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Command",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "AutoModPluginId",
                table: "Command",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Command_AutoModPluginId",
                table: "Command",
                column: "AutoModPluginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Command_AutoModPlugin_AutoModPluginId",
                table: "Command",
                column: "AutoModPluginId",
                principalTable: "AutoModPlugin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
