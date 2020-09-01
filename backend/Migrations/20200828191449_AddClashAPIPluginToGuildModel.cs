using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace backend.Migrations
{
    public partial class AddClashAPIPluginToGuildModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClashAPIPluginId",
                table: "Guilds",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClashAPIPlugin",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClashAPIPlugin", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_ClashAPIPluginId",
                table: "Guilds",
                column: "ClashAPIPluginId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_ClashAPIPlugin_ClashAPIPluginId",
                table: "Guilds",
                column: "ClashAPIPluginId",
                principalTable: "ClashAPIPlugin",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_ClashAPIPlugin_ClashAPIPluginId",
                table: "Guilds");

            migrationBuilder.DropTable(
                name: "ClashAPIPlugin");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_ClashAPIPluginId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "ClashAPIPluginId",
                table: "Guilds");
        }
    }
}
