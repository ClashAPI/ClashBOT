using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class AddScheduledMessagesPlugin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ScheduledMessagesPluginId",
                table: "ScheduledMessage",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduledMessagesPluginId",
                table: "Guilds",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScheduledMessagesPlugin",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledMessagesPlugin", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledMessage_ScheduledMessagesPluginId",
                table: "ScheduledMessage",
                column: "ScheduledMessagesPluginId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_ScheduledMessagesPluginId",
                table: "Guilds",
                column: "ScheduledMessagesPluginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_ScheduledMessagesPlugin_ScheduledMessagesPluginId",
                table: "Guilds",
                column: "ScheduledMessagesPluginId",
                principalTable: "ScheduledMessagesPlugin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ScheduledMessage_ScheduledMessagesPlugin_ScheduledMessagesPluginId",
                table: "ScheduledMessage",
                column: "ScheduledMessagesPluginId",
                principalTable: "ScheduledMessagesPlugin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_ScheduledMessagesPlugin_ScheduledMessagesPluginId",
                table: "Guilds");

            migrationBuilder.DropForeignKey(
                name: "FK_ScheduledMessage_ScheduledMessagesPlugin_ScheduledMessagesPluginId",
                table: "ScheduledMessage");

            migrationBuilder.DropTable(
                name: "ScheduledMessagesPlugin");

            migrationBuilder.DropIndex(
                name: "IX_ScheduledMessage_ScheduledMessagesPluginId",
                table: "ScheduledMessage");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_ScheduledMessagesPluginId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "ScheduledMessagesPluginId",
                table: "ScheduledMessage");

            migrationBuilder.DropColumn(
                name: "ScheduledMessagesPluginId",
                table: "Guilds");
        }
    }
}
