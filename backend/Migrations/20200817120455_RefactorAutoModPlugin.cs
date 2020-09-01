using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace backend.Migrations
{
    public partial class RefactorAutoModPlugin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlacklistedWord");

            migrationBuilder.AddColumn<Guid>(
                name: "BadWordsSettingsId",
                table: "AutoModPlugin",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExcessiveCapsSettingsId",
                table: "AutoModPlugin",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExcessiveEmojisSettingsId",
                table: "AutoModPlugin",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExcessiveMentionsSettingsId",
                table: "AutoModPlugin",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExcessiveSpoilersSettingsId",
                table: "AutoModPlugin",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExternalLinksSettingsId",
                table: "AutoModPlugin",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IgnoreBots",
                table: "AutoModPlugin",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RepeatedTextSettingsId",
                table: "AutoModPlugin",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServerInvitesSettingsId",
                table: "AutoModPlugin",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ZalgoSettingsId",
                table: "AutoModPlugin",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AutomatedAction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModerationAction = table.Column<int>(nullable: false),
                    InfractionsLimit = table.Column<int>(nullable: false),
                    TimeLimitInSeconds = table.Column<int>(nullable: false),
                    AutoModPluginId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomatedAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutomatedAction_AutoModPlugin_AutoModPluginId",
                        column: x => x.AutoModPluginId,
                        principalTable: "AutoModPlugin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BadWordsSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadWordsSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BadWordsSettings_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcessiveCapsSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcessiveCapsSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcessiveCapsSettings_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcessiveEmojisSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true),
                    EmojiLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcessiveEmojisSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcessiveEmojisSettings_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcessiveMentionsSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true),
                    MentionsLimit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcessiveMentionsSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcessiveMentionsSettings_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcessiveSpoilersSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpoilerTagsLimit = table.Column<int>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcessiveSpoilersSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcessiveSpoilersSettings_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExternalLinksSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalLinksSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExternalLinksSettings_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RepeatedTextSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepeatedTextSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepeatedTextSettings_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServerInvitesSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerInvitesSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServerInvitesSettings_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZalgoSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GuildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZalgoSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZalgoSettings_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BadWord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Word = table.Column<string>(nullable: true),
                    GuildId = table.Column<Guid>(nullable: true),
                    BadWordsSettingsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BadWord_BadWordsSettings_BadWordsSettingsId",
                        column: x => x.BadWordsSettingsId,
                        principalTable: "BadWordsSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BadWord_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    ExternalLinksSettingsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Website_ExternalLinksSettings_ExternalLinksSettingsId",
                        column: x => x.ExternalLinksSettingsId,
                        principalTable: "ExternalLinksSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscordChannel",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<string>(nullable: true),
                    BadWordsSettingsId = table.Column<Guid>(nullable: true),
                    ExcessiveCapsSettingsId = table.Column<Guid>(nullable: true),
                    ExcessiveEmojisSettingsId = table.Column<Guid>(nullable: true),
                    ExcessiveMentionsSettingsId = table.Column<Guid>(nullable: true),
                    ExcessiveSpoilersSettingsId = table.Column<Guid>(nullable: true),
                    ExternalLinksSettingsId = table.Column<Guid>(nullable: true),
                    RepeatedTextSettingsId = table.Column<Guid>(nullable: true),
                    ServerInvitesSettingsId = table.Column<Guid>(nullable: true),
                    ZalgoSettingsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_BadWordsSettings_BadWordsSettingsId",
                        column: x => x.BadWordsSettingsId,
                        principalTable: "BadWordsSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_ExcessiveCapsSettings_ExcessiveCapsSettingsId",
                        column: x => x.ExcessiveCapsSettingsId,
                        principalTable: "ExcessiveCapsSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_ExcessiveEmojisSettings_ExcessiveEmojisSettingsId",
                        column: x => x.ExcessiveEmojisSettingsId,
                        principalTable: "ExcessiveEmojisSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_ExcessiveMentionsSettings_ExcessiveMentionsSettingsId",
                        column: x => x.ExcessiveMentionsSettingsId,
                        principalTable: "ExcessiveMentionsSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_ExcessiveSpoilersSettings_ExcessiveSpoilersSettingsId",
                        column: x => x.ExcessiveSpoilersSettingsId,
                        principalTable: "ExcessiveSpoilersSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_ExternalLinksSettings_ExternalLinksSettingsId",
                        column: x => x.ExternalLinksSettingsId,
                        principalTable: "ExternalLinksSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_RepeatedTextSettings_RepeatedTextSettingsId",
                        column: x => x.RepeatedTextSettingsId,
                        principalTable: "RepeatedTextSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_ServerInvitesSettings_ServerInvitesSettingsId",
                        column: x => x.ServerInvitesSettingsId,
                        principalTable: "ServerInvitesSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordChannel_ZalgoSettings_ZalgoSettingsId",
                        column: x => x.ZalgoSettingsId,
                        principalTable: "ZalgoSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DiscordRole",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    BadWordsSettingsId = table.Column<Guid>(nullable: true),
                    ExcessiveCapsSettingsId = table.Column<Guid>(nullable: true),
                    ExcessiveEmojisSettingsId = table.Column<Guid>(nullable: true),
                    ExcessiveMentionsSettingsId = table.Column<Guid>(nullable: true),
                    ExcessiveSpoilersSettingsId = table.Column<Guid>(nullable: true),
                    ExternalLinksSettingsId = table.Column<Guid>(nullable: true),
                    RepeatedTextSettingsId = table.Column<Guid>(nullable: true),
                    ServerInvitesSettingsId = table.Column<Guid>(nullable: true),
                    ZalgoSettingsId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordRole_BadWordsSettings_BadWordsSettingsId",
                        column: x => x.BadWordsSettingsId,
                        principalTable: "BadWordsSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordRole_ExcessiveCapsSettings_ExcessiveCapsSettingsId",
                        column: x => x.ExcessiveCapsSettingsId,
                        principalTable: "ExcessiveCapsSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordRole_ExcessiveEmojisSettings_ExcessiveEmojisSettingsId",
                        column: x => x.ExcessiveEmojisSettingsId,
                        principalTable: "ExcessiveEmojisSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordRole_ExcessiveMentionsSettings_ExcessiveMentionsSettingsId",
                        column: x => x.ExcessiveMentionsSettingsId,
                        principalTable: "ExcessiveMentionsSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordRole_ExcessiveSpoilersSettings_ExcessiveSpoilersSettingsId",
                        column: x => x.ExcessiveSpoilersSettingsId,
                        principalTable: "ExcessiveSpoilersSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordRole_ExternalLinksSettings_ExternalLinksSettingsId",
                        column: x => x.ExternalLinksSettingsId,
                        principalTable: "ExternalLinksSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordRole_RepeatedTextSettings_RepeatedTextSettingsId",
                        column: x => x.RepeatedTextSettingsId,
                        principalTable: "RepeatedTextSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordRole_ServerInvitesSettings_ServerInvitesSettingsId",
                        column: x => x.ServerInvitesSettingsId,
                        principalTable: "ServerInvitesSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DiscordRole_ZalgoSettings_ZalgoSettingsId",
                        column: x => x.ZalgoSettingsId,
                        principalTable: "ZalgoSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoModPlugin_BadWordsSettingsId",
                table: "AutoModPlugin",
                column: "BadWordsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoModPlugin_ExcessiveCapsSettingsId",
                table: "AutoModPlugin",
                column: "ExcessiveCapsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoModPlugin_ExcessiveEmojisSettingsId",
                table: "AutoModPlugin",
                column: "ExcessiveEmojisSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoModPlugin_ExcessiveMentionsSettingsId",
                table: "AutoModPlugin",
                column: "ExcessiveMentionsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoModPlugin_ExcessiveSpoilersSettingsId",
                table: "AutoModPlugin",
                column: "ExcessiveSpoilersSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoModPlugin_ExternalLinksSettingsId",
                table: "AutoModPlugin",
                column: "ExternalLinksSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoModPlugin_RepeatedTextSettingsId",
                table: "AutoModPlugin",
                column: "RepeatedTextSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoModPlugin_ServerInvitesSettingsId",
                table: "AutoModPlugin",
                column: "ServerInvitesSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutoModPlugin_ZalgoSettingsId",
                table: "AutoModPlugin",
                column: "ZalgoSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomatedAction_AutoModPluginId",
                table: "AutomatedAction",
                column: "AutoModPluginId");

            migrationBuilder.CreateIndex(
                name: "IX_BadWord_BadWordsSettingsId",
                table: "BadWord",
                column: "BadWordsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_BadWord_GuildId",
                table: "BadWord",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_BadWordsSettings_GuildId",
                table: "BadWordsSettings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_BadWordsSettingsId",
                table: "DiscordChannel",
                column: "BadWordsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_ExcessiveCapsSettingsId",
                table: "DiscordChannel",
                column: "ExcessiveCapsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_ExcessiveEmojisSettingsId",
                table: "DiscordChannel",
                column: "ExcessiveEmojisSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_ExcessiveMentionsSettingsId",
                table: "DiscordChannel",
                column: "ExcessiveMentionsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_ExcessiveSpoilersSettingsId",
                table: "DiscordChannel",
                column: "ExcessiveSpoilersSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_ExternalLinksSettingsId",
                table: "DiscordChannel",
                column: "ExternalLinksSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_RepeatedTextSettingsId",
                table: "DiscordChannel",
                column: "RepeatedTextSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_ServerInvitesSettingsId",
                table: "DiscordChannel",
                column: "ServerInvitesSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordChannel_ZalgoSettingsId",
                table: "DiscordChannel",
                column: "ZalgoSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_BadWordsSettingsId",
                table: "DiscordRole",
                column: "BadWordsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_ExcessiveCapsSettingsId",
                table: "DiscordRole",
                column: "ExcessiveCapsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_ExcessiveEmojisSettingsId",
                table: "DiscordRole",
                column: "ExcessiveEmojisSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_ExcessiveMentionsSettingsId",
                table: "DiscordRole",
                column: "ExcessiveMentionsSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_ExcessiveSpoilersSettingsId",
                table: "DiscordRole",
                column: "ExcessiveSpoilersSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_ExternalLinksSettingsId",
                table: "DiscordRole",
                column: "ExternalLinksSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_RepeatedTextSettingsId",
                table: "DiscordRole",
                column: "RepeatedTextSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_ServerInvitesSettingsId",
                table: "DiscordRole",
                column: "ServerInvitesSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRole_ZalgoSettingsId",
                table: "DiscordRole",
                column: "ZalgoSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcessiveCapsSettings_GuildId",
                table: "ExcessiveCapsSettings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcessiveEmojisSettings_GuildId",
                table: "ExcessiveEmojisSettings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcessiveMentionsSettings_GuildId",
                table: "ExcessiveMentionsSettings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcessiveSpoilersSettings_GuildId",
                table: "ExcessiveSpoilersSettings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_ExternalLinksSettings_GuildId",
                table: "ExternalLinksSettings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_RepeatedTextSettings_GuildId",
                table: "RepeatedTextSettings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerInvitesSettings_GuildId",
                table: "ServerInvitesSettings",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Website_ExternalLinksSettingsId",
                table: "Website",
                column: "ExternalLinksSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ZalgoSettings_GuildId",
                table: "ZalgoSettings",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutoModPlugin_BadWordsSettings_BadWordsSettingsId",
                table: "AutoModPlugin",
                column: "BadWordsSettingsId",
                principalTable: "BadWordsSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoModPlugin_ExcessiveCapsSettings_ExcessiveCapsSettingsId",
                table: "AutoModPlugin",
                column: "ExcessiveCapsSettingsId",
                principalTable: "ExcessiveCapsSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoModPlugin_ExcessiveEmojisSettings_ExcessiveEmojisSettingsId",
                table: "AutoModPlugin",
                column: "ExcessiveEmojisSettingsId",
                principalTable: "ExcessiveEmojisSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoModPlugin_ExcessiveMentionsSettings_ExcessiveMentionsSettingsId",
                table: "AutoModPlugin",
                column: "ExcessiveMentionsSettingsId",
                principalTable: "ExcessiveMentionsSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoModPlugin_ExcessiveSpoilersSettings_ExcessiveSpoilersSettingsId",
                table: "AutoModPlugin",
                column: "ExcessiveSpoilersSettingsId",
                principalTable: "ExcessiveSpoilersSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoModPlugin_ExternalLinksSettings_ExternalLinksSettingsId",
                table: "AutoModPlugin",
                column: "ExternalLinksSettingsId",
                principalTable: "ExternalLinksSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoModPlugin_RepeatedTextSettings_RepeatedTextSettingsId",
                table: "AutoModPlugin",
                column: "RepeatedTextSettingsId",
                principalTable: "RepeatedTextSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoModPlugin_ServerInvitesSettings_ServerInvitesSettingsId",
                table: "AutoModPlugin",
                column: "ServerInvitesSettingsId",
                principalTable: "ServerInvitesSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AutoModPlugin_ZalgoSettings_ZalgoSettingsId",
                table: "AutoModPlugin",
                column: "ZalgoSettingsId",
                principalTable: "ZalgoSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutoModPlugin_BadWordsSettings_BadWordsSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoModPlugin_ExcessiveCapsSettings_ExcessiveCapsSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoModPlugin_ExcessiveEmojisSettings_ExcessiveEmojisSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoModPlugin_ExcessiveMentionsSettings_ExcessiveMentionsSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoModPlugin_ExcessiveSpoilersSettings_ExcessiveSpoilersSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoModPlugin_ExternalLinksSettings_ExternalLinksSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoModPlugin_RepeatedTextSettings_RepeatedTextSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoModPlugin_ServerInvitesSettings_ServerInvitesSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropForeignKey(
                name: "FK_AutoModPlugin_ZalgoSettings_ZalgoSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropTable(
                name: "AutomatedAction");

            migrationBuilder.DropTable(
                name: "BadWord");

            migrationBuilder.DropTable(
                name: "DiscordChannel");

            migrationBuilder.DropTable(
                name: "DiscordRole");

            migrationBuilder.DropTable(
                name: "Website");

            migrationBuilder.DropTable(
                name: "BadWordsSettings");

            migrationBuilder.DropTable(
                name: "ExcessiveCapsSettings");

            migrationBuilder.DropTable(
                name: "ExcessiveEmojisSettings");

            migrationBuilder.DropTable(
                name: "ExcessiveMentionsSettings");

            migrationBuilder.DropTable(
                name: "ExcessiveSpoilersSettings");

            migrationBuilder.DropTable(
                name: "RepeatedTextSettings");

            migrationBuilder.DropTable(
                name: "ServerInvitesSettings");

            migrationBuilder.DropTable(
                name: "ZalgoSettings");

            migrationBuilder.DropTable(
                name: "ExternalLinksSettings");

            migrationBuilder.DropIndex(
                name: "IX_AutoModPlugin_BadWordsSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropIndex(
                name: "IX_AutoModPlugin_ExcessiveCapsSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropIndex(
                name: "IX_AutoModPlugin_ExcessiveEmojisSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropIndex(
                name: "IX_AutoModPlugin_ExcessiveMentionsSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropIndex(
                name: "IX_AutoModPlugin_ExcessiveSpoilersSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropIndex(
                name: "IX_AutoModPlugin_ExternalLinksSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropIndex(
                name: "IX_AutoModPlugin_RepeatedTextSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropIndex(
                name: "IX_AutoModPlugin_ServerInvitesSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropIndex(
                name: "IX_AutoModPlugin_ZalgoSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "BadWordsSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "ExcessiveCapsSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "ExcessiveEmojisSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "ExcessiveMentionsSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "ExcessiveSpoilersSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "ExternalLinksSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "IgnoreBots",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "RepeatedTextSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "ServerInvitesSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.DropColumn(
                name: "ZalgoSettingsId",
                table: "AutoModPlugin");

            migrationBuilder.CreateTable(
                name: "BlacklistedWord",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AutoModPluginId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuildId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Word = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlacklistedWord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlacklistedWord_AutoModPlugin_AutoModPluginId",
                        column: x => x.AutoModPluginId,
                        principalTable: "AutoModPlugin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlacklistedWord_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedWord_AutoModPluginId",
                table: "BlacklistedWord",
                column: "AutoModPluginId");

            migrationBuilder.CreateIndex(
                name: "IX_BlacklistedWord_GuildId",
                table: "BlacklistedWord",
                column: "GuildId");
        }
    }
}
