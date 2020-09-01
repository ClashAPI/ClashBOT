using Microsoft.EntityFrameworkCore.Migrations;

namespace backend.Migrations
{
    public partial class AddModerationActionToPluginSettingsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ModerationAction",
                table: "ZalgoSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModerationAction",
                table: "ServerInvitesSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModerationAction",
                table: "RepeatedTextSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModerationAction",
                table: "ExternalLinksSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModerationAction",
                table: "ExcessiveSpoilersSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModerationAction",
                table: "ExcessiveMentionsSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModerationAction",
                table: "ExcessiveEmojisSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModerationAction",
                table: "ExcessiveCapsSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ModerationAction",
                table: "BadWordsSettings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModerationAction",
                table: "ZalgoSettings");

            migrationBuilder.DropColumn(
                name: "ModerationAction",
                table: "ServerInvitesSettings");

            migrationBuilder.DropColumn(
                name: "ModerationAction",
                table: "RepeatedTextSettings");

            migrationBuilder.DropColumn(
                name: "ModerationAction",
                table: "ExternalLinksSettings");

            migrationBuilder.DropColumn(
                name: "ModerationAction",
                table: "ExcessiveSpoilersSettings");

            migrationBuilder.DropColumn(
                name: "ModerationAction",
                table: "ExcessiveMentionsSettings");

            migrationBuilder.DropColumn(
                name: "ModerationAction",
                table: "ExcessiveEmojisSettings");

            migrationBuilder.DropColumn(
                name: "ModerationAction",
                table: "ExcessiveCapsSettings");

            migrationBuilder.DropColumn(
                name: "ModerationAction",
                table: "BadWordsSettings");
        }
    }
}
