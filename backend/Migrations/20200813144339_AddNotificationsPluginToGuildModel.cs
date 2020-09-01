using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace backend.Migrations
{
    public partial class AddNotificationsPluginToGuildModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NotificationsPluginId",
                table: "Guilds",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationsPlugin",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsPlugin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TwitchNotification",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    ChannelId = table.Column<string>(nullable: true),
                    NotificationsPluginId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchNotification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TwitchNotification_NotificationsPlugin_NotificationsPluginId",
                        column: x => x.NotificationsPluginId,
                        principalTable: "NotificationsPlugin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_NotificationsPluginId",
                table: "Guilds",
                column: "NotificationsPluginId");

            migrationBuilder.CreateIndex(
                name: "IX_TwitchNotification_NotificationsPluginId",
                table: "TwitchNotification",
                column: "NotificationsPluginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_NotificationsPlugin_NotificationsPluginId",
                table: "Guilds",
                column: "NotificationsPluginId",
                principalTable: "NotificationsPlugin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_NotificationsPlugin_NotificationsPluginId",
                table: "Guilds");

            migrationBuilder.DropTable(
                name: "TwitchNotification");

            migrationBuilder.DropTable(
                name: "NotificationsPlugin");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_NotificationsPluginId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "NotificationsPluginId",
                table: "Guilds");
        }
    }
}
