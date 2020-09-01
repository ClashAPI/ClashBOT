using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace backend.Migrations
{
    public partial class RemoveGuildsFromUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_AspNetUsers_UserId",
                table: "Guilds");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_UserId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Guilds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Guilds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_UserId",
                table: "Guilds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_AspNetUsers_UserId",
                table: "Guilds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
