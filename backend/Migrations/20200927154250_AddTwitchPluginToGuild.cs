using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class AddTwitchPluginToGuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TwitchPluginId",
                table: "TwitchChannelSubscription",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TwitchPluginId",
                table: "Guilds",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TwitchPlugin",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitchPlugin", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TwitchChannelSubscription_TwitchPluginId",
                table: "TwitchChannelSubscription",
                column: "TwitchPluginId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_TwitchPluginId",
                table: "Guilds",
                column: "TwitchPluginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_TwitchPlugin_TwitchPluginId",
                table: "Guilds",
                column: "TwitchPluginId",
                principalTable: "TwitchPlugin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TwitchChannelSubscription_TwitchPlugin_TwitchPluginId",
                table: "TwitchChannelSubscription",
                column: "TwitchPluginId",
                principalTable: "TwitchPlugin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_TwitchPlugin_TwitchPluginId",
                table: "Guilds");

            migrationBuilder.DropForeignKey(
                name: "FK_TwitchChannelSubscription_TwitchPlugin_TwitchPluginId",
                table: "TwitchChannelSubscription");

            migrationBuilder.DropTable(
                name: "TwitchPlugin");

            migrationBuilder.DropIndex(
                name: "IX_TwitchChannelSubscription_TwitchPluginId",
                table: "TwitchChannelSubscription");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_TwitchPluginId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "TwitchPluginId",
                table: "TwitchChannelSubscription");

            migrationBuilder.DropColumn(
                name: "TwitchPluginId",
                table: "Guilds");
        }
    }
}
