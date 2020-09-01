using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace backend.Migrations
{
    public partial class AddManagersToGuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GuildId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GuildId",
                table: "AspNetUsers",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Guilds_GuildId",
                table: "AspNetUsers",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Guilds_GuildId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GuildId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "AspNetUsers");
        }
    }
}
