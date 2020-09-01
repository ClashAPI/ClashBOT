using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace backend.Migrations
{
    public partial class AddModeratorCommandsToAutoModPlugin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Command",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "AutoModPluginId",
                table: "Command",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
