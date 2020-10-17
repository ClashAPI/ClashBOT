using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class AddPreferencesToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppPreferencesId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Theme = table.Column<int>(nullable: false),
                    Language = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppPreferences", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AppPreferencesId",
                table: "AspNetUsers",
                column: "AppPreferencesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AppPreferences_AppPreferencesId",
                table: "AspNetUsers",
                column: "AppPreferencesId",
                principalTable: "AppPreferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AppPreferences_AppPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AppPreferences");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AppPreferencesId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AppPreferencesId",
                table: "AspNetUsers");
        }
    }
}
