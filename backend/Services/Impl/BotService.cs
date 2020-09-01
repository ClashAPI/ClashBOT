using backend.DTOs;
using backend.Models;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.CompilerServices;
using Command = backend.Models.Command;
using DiscordChannel = DSharpPlus.Entities.DiscordChannel;
using DiscordRole = DSharpPlus.Entities.DiscordRole;

// TODO: Create an init method which creates the scope and gets the guild
namespace backend.Services
{
    public class BotService : IBotService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Bot _bot;

        public BotService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _bot = serviceProvider.GetRequiredService<Bot>();
            _bot.Client.MessageCreated += OnMessageCreated;
            _bot.Client.GuildMemberAdded += OnGuildMemberAdded;
        }

        private async Task<bool> CheckPermissionsAsync(DiscordGuild guild, User user)
        {
            var owner = guild.Owner;
            if (owner.Username + owner.Discriminator == user.UserName + user.Discriminator)
            {
                return true;
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                var dbGuild = await guildService.GetByGuildIdAsync(guild.Id.ToString());


                if (dbGuild.Managers.Select(m => m.UserId).ToList().Contains(user.UserId))
                {
                    return true;
                }

                return false;
            }
        }

        private async Task<bool> CheckIfHasPermissionAsync(DiscordUser discordUser, DiscordGuild discordGuild,
            Guild dbGuild)
        {
            var hasPermission = false;
            var invoker = await GetMemberAsync(dbGuild.GuildId, discordUser.Id);

            if (invoker.IsOwner)
            {
                hasPermission = true;
            }

            if (!hasPermission)
            {
                var managerIds = dbGuild.Managers.Select(m => m.UserId).ToList();
                if (managerIds.Contains(invoker.Id.ToString()))
                {
                    hasPermission = true;
                }
            }

            if (!hasPermission)
            {
                var moderatorRoleIds = dbGuild.AutoModPlugin.ModeratorRoles.Select(r => r.RoleId).ToList();
                foreach (var roleId in invoker.Roles.Select(r => r.Id).ToList())
                {
                    if (moderatorRoleIds.Contains(roleId.ToString()))
                    {
                        hasPermission = true;
                        break;
                    }
                }
            }

            return hasPermission;
        }

        private async Task BanCommandAsync(MessageCreateEventArgs e, Guild guild)
        {
            if (guild.AutoModPlugin.ModeratorCommands.First(c => c.CommandCall == "ban").IsEnabled)
            {
                var hasPermission = await CheckIfHasPermissionAsync(e.Author, e.Guild, guild);

                if (hasPermission)
                {
                    if (e.Message.Content.Trim().Length == 4)
                    {
                        var discordEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Azure,
                            Title = "Ban",
                            Description = "Ban member from the guild",
                        };
                        discordEmbed.AddField("Usage", "Mention a member to ban");
                        discordEmbed.AddField("Example", "!ban @ClashAPI");

                        await e.Message.RespondAsync(embed: discordEmbed);
                    }
                    else
                    {
                        var members = e.Message.MentionedUsers;

                        try
                        {
                            foreach (var member in members)
                            {
                                await e.Guild.BanMemberAsync(member.Id);
                            }

                            var successEmbed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Green,
                                Title = "Operation successful",
                                Description = "Member has been banned successfully",
                                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = "Unban", IconUrl = "" },
                            };

                            await e.Message.RespondAsync(embed: successEmbed);
                        }
                        catch
                        {
                            var errorEmbed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Red,
                                Title = "Operation failed",
                                Description = "Member has not been banned",
                            };

                            await e.Message.RespondAsync(embed: errorEmbed);
                        }
                    }
                }
            }
        }

        private async Task KickCommandAsync(MessageCreateEventArgs e, Guild guild)
        {
            if (guild.AutoModPlugin.ModeratorCommands.First(c => c.CommandCall == "kick").IsEnabled)
            {
                var hasPermission = await CheckIfHasPermissionAsync(e.Author, e.Guild, guild);

                if (hasPermission)
                {
                    if (e.Message.Content.Trim().Length == 5)
                    {
                        var discordEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Azure,
                            Title = "Kick",
                            Description = "Kick a member from the guild",
                        };
                        discordEmbed.AddField("Usage", "Mention a member to kick");
                        discordEmbed.AddField("Example", "!kick @ClashAPI");

                        await e.Message.RespondAsync(embed: discordEmbed);
                    }
                    else
                    {
                        var members = e.Message.MentionedUsers;

                        try
                        {
                            foreach (var member in members)
                            {
                                await e.Guild.RemoveMemberAsync(await e.Guild.GetMemberAsync(member.Id));
                            }

                            var successEmbed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Green,
                                Title = "Operation successful",
                                Description = "Member has been kicked successfully",
                            };

                            await e.Message.RespondAsync(embed: successEmbed);
                        }
                        catch
                        {
                            var errorEmbed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Red,
                                Title = "Operation failed",
                                Description = "Member has not been kicked",
                            };

                            await e.Message.RespondAsync(embed: errorEmbed);
                        }
                    }
                }
            }
        }

        private async Task TempbanCommandAsync(MessageCreateEventArgs e, Guild guild)
        {
            if (guild.AutoModPlugin.ModeratorCommands.First(c => c.CommandCall == "tempban").IsEnabled)
            {
                var hasPermission = await CheckIfHasPermissionAsync(e.Author, e.Guild, guild);

                if (hasPermission)
                {
                    if (e.Message.Content.Trim().Length == 8)
                    {
                        var discordEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Azure,
                            Title = "Temporary ban",
                            Description = "Ban member temporarily from the guild",
                        };
                        discordEmbed.AddField("Usage", "Mention a member to ban, and the duration in seconds");
                        discordEmbed.AddField("Example", "!tempban @ClashAPI 3600");

                        await e.Message.RespondAsync(embed: discordEmbed);
                    }
                    else
                    {
                        var members = e.Message.MentionedUsers;

                        try
                        {
                            foreach (var member in members)
                            {
                                await e.Guild.RemoveMemberAsync(await e.Guild.GetMemberAsync(member.Id));
                            }

                            var successEmbed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Green,
                                Title = "Operation successful",
                                Description = "Member has been banned successfully",
                                Footer = new DiscordEmbedBuilder.EmbedFooter {Text = "Unban", IconUrl = ""},
                            };

                            await e.Message.RespondAsync(embed: successEmbed);
                        }
                        catch
                        {
                            var errorEmbed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Red,
                                Title = "Operation failed",
                                Description = "Member has not been banned",
                            };

                            await e.Message.RespondAsync(embed: errorEmbed);
                        }
                    }
                }
            }
        }

        private async Task ClearCommandAsync(MessageCreateEventArgs e, Guild guild)
        {
            var hasPermission = await CheckIfHasPermissionAsync(e.Author, e.Guild, guild);

            if (hasPermission)
            {
                try
                {
                    if (guild.AutoModPlugin.ModeratorCommands.First(c => c.CommandCall == "clear").IsEnabled)
                    {
                        await e.Channel.DeleteMessagesAsync(await e.Channel.GetMessagesAsync());

                        var successEmbed = new DiscordEmbedBuilder
                        {
                            Color = DiscordColor.Green,
                            Title = "Operation successful",
                            Description = "Channel messages have been deleted successfully",
                        };

                        await e.Message.RespondAsync(embed: successEmbed);
                    }
                }
                catch (Exception exception)
                {
                    var errorEmbed = new DiscordEmbedBuilder
                    {
                        Color = DiscordColor.Red,
                        Title = "Operation failed",
                        Description = "Channel messages could not be deleted",
                    };

                    await e.Message.RespondAsync(embed: errorEmbed);
                    Console.WriteLine(exception);
                }
            }
        }

        private async Task FilterMessageAsync(MessageCreateEventArgs e, Guild guild, string[] words, List<string> blacklistedWords)
        {
            foreach (var word in words)
            {
                if (blacklistedWords.Contains(word))
                {
                    var member = await e.Guild.GetMemberAsync(e.Author.Id);
                    var modRoleIds = guild.AutoModPlugin.ModeratorRoles
                        .Select(r => r.RoleId)
                        .ToList();
                    var memberRoleIds = member.Roles
                        .Select(r => r.Id)
                        .ToList();
                    var isImmune = false;

                    if (guild.AutoModPlugin.BadWordsSettings.IgnoredChannels
                        .Select(c => c.ChannelId)
                        .ToList()
                        .Contains(e.Channel.Id.ToString()))
                    {
                        isImmune = true;
                    }

                    if (!isImmune)
                    {
                        var allowedRoleIds = guild.AutoModPlugin.BadWordsSettings.AllowedRoles.Select(r => r.RoleId)
                            .ToList();
                        foreach (var allowedRoleId in allowedRoleIds)
                        {
                            if (member.Roles.Select(r => r.Id.ToString()).ToList().Contains(allowedRoleId))
                            {
                                isImmune = true;
                                break;
                            }
                        }
                    }

                    if (!isImmune)
                    {
                        foreach (var modRoleId in modRoleIds)
                        {
                            if (memberRoleIds.Contains(ulong.Parse(modRoleId)))
                            {
                                isImmune = true;
                                break;
                            }
                        }
                    }

                    if (!isImmune)
                    {
                        if (guild.AutoModPlugin.BadWordsSettings.ModerationAction == ModerationAction.DeleteMessage ||
                            guild.AutoModPlugin.BadWordsSettings.ModerationAction ==
                            ModerationAction.DeleteMessageAndWarnMember)
                        {
                            await e.Message.DeleteAsync();
                        }

                        if (guild.AutoModPlugin.BadWordsSettings.ModerationAction == ModerationAction.WarnMember ||
                            guild.AutoModPlugin.BadWordsSettings.ModerationAction ==
                            ModerationAction.DeleteMessageAndWarnMember)
                        {
                            guild.Infractions.Add(new Infraction
                            {
                                Guild = guild,
                                MemberId = e.Author.Id.ToString(),
                                Date = new DateTimeOffset()
                            });

                            // TODO: If infractions are enabled, register an infraction, and show the number
                            // TODO: of active infractions in a field
                            var discordEmbed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Red,
                                Title = "Warning",
                                Description =
                                    "You have been warned for using forbidden word(s)",
                            };

                            await e.Channel.SendMessageAsync(content: e.Author.Mention, embed: discordEmbed);
                        }
                    }
                }
            }
        }

        private async Task EvaluateCustomCommands(MessageCreateEventArgs e, IList<Command> commands, IList<AdvancedCommand> advancedCommands)
        {
            var enabledCommands = commands.Where(c => c.IsEnabled).ToList();

            foreach (var command in enabledCommands)
            {
                if (e.Message.Content.Equals(command.Prefix + command.CommandCall))
                {
                    var message = command.Response;
                    message = message.Replace("$USERNAME", e.Author.Username);
                    message = message.Replace("$MENTION", e.Author.Mention);

                    await e.Channel.SendMessageAsync(message);
                }
            }

            foreach (var command in advancedCommands.Where(c => c.IsEnabled).ToList())
            {
                if (e.Message.Content.Equals(command.Prefix + command.CommandCall))
                {
                    DiscordMember member = null;

                    foreach (var action in command.Actions)
                    {
                        if (action.Type == CommandActionType.SendMessage)
                        {
                            var message = action.Message;
                            message = message.Replace("$USERNAME", e.Author.Username);
                            message = message.Replace("$MENTION", e.Author.Mention);

                            await e.Channel.SendMessageAsync(message);
                        }


                        if (action.Type == CommandActionType.AddRole)
                        {
                            if (member == null)
                            {
                                member = await e.Guild.GetMemberAsync(e.Author.Id);
                            }

                            var role = e.Guild.GetRole(ulong.Parse(action.RoleId));
                            await member.GrantRoleAsync(role);
                        }

                        if (action.Type == CommandActionType.RemoveRole)
                        {
                            if (member == null)
                            {
                                member = await e.Guild.GetMemberAsync(e.Author.Id);
                            }

                            var role = e.Guild.GetRole(ulong.Parse(action.RoleId));
                            await member.RevokeRoleAsync(role);
                        }
                    }
                }
            }
        }

        // TODO: Custom commands could be either defined in the database
        // and be registered manually or could be added here
        // UPDATE: Commands can not be registered programatically (f.e. in Bot.cs) UPDATE2: couldn't it?
        private async Task OnMessageCreated(MessageCreateEventArgs e)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                    // TODO: Improve | details: https://clashapi.atlassian.net/jira/software/projects/BOT/boards/5?selectedIssue=BOT-32
                    var guild = await guildService.GetByGuildIdAsync(e.Guild.Id.ToString());

                    if (guild.CustomCommandPlugin.IsEnabled)
                    {
                        await EvaluateCustomCommands(e, guild.CustomCommandPlugin.Commands, guild.CustomCommandPlugin.AdvancedCommands);
                    }

                    if (guild.AutoModPlugin.IsEnabled)
                    {
                        var blacklistedWords = guild.AutoModPlugin.BadWordsSettings.BadWords
                            .Select(b => b.Word)
                            .ToList();

                        var words = e.Message.Content.Split(' ');

                        // Check if the command is enabled
                        if (true)
                        {
                            if (words[0] == "!ban")
                            {
                                await BanCommandAsync(e, guild);
                            }
                        }

                        // Check if the command is enabled
                        if (true)
                        {
                            if (words[0] == "!kick")
                            {
                                await KickCommandAsync(e, guild);
                            }
                        }

                        // Check if the command is enabled
                        if (true)
                        {
                            if (words[0] == "!tempban")
                            {
                                await TempbanCommandAsync(e, guild);
                            }
                        }

                        // Check if the command is enabled
                        if (true)
                        {
                            if (words[0] == "!clear")
                            {
                                await ClearCommandAsync(e, guild);
                            }
                        }

                        // DsharpPlus API upstream does not support this yet
                        /*
                        if (true)
                        {
                            if (words[0] == "!slowmode")
                            {
                                try
                                {
                                    var successEmbed = new DiscordEmbedBuilder
                                    {
                                        Color = DiscordColor.Green,
                                        Title = "Operation successful",
                                        Description = "Channel messages have been deleted successfully",
                                    };
                                    
                                    await e.Message.RespondAsync(embed: successEmbed);
                                }
                                catch
                                {
                                    var errorEmbed = new DiscordEmbedBuilder
                                    {
                                        Color = DiscordColor.Red,
                                        Title = "Operation failed",
                                        Description = "Channel messages could not be deleted",
                                    };
                                    
                                    await e.Message.RespondAsync(embed: errorEmbed);
                                }
                            }
                        }
                        */

                        // If Clash Royale plugin | command is enabled
                        if (true)
                        {
                            var clashRoyaleService = scope.ServiceProvider.GetRequiredService<IClashRoyaleService>();
                            var baseUrl = "https://clashapi.net/";

                            if (words[0] == "!player" && e.Message.Content.Trim().Length > 7)
                            {
                                var message = e.Message.Content.Split(' ');
                                var player = await clashRoyaleService.GetPlayerAsync(message[1]);

                                var playerLink = baseUrl + $"player/{player.Tag.TrimStart('#')}";
                                var clanLink = "";

                                if (player.Clan != null)
                                {
                                    clanLink = baseUrl + $"clan/{player.Clan.Tag.TrimStart('#')}";
                                }

                                var rawArenaId = player.Arena.Id.ToString();
                                var arenaId = Convert.ToInt32(rawArenaId.Substring(6, 2)) + 1;

                                var draws = player.BattleCount - (player.Wins + player.Losses);

                                var description = new StringBuilder();
                                description.Append(
                                    $"[{player.Tag}]({playerLink})\n");

                                if (player.Clan != null)
                                {
                                    description.Append($"[{player.Clan.Name}]({clanLink})\n[{player.Clan.Tag}]({clanLink})\n{clashRoyaleService.GetNormalizedClanRole(player.Role)}");
                                }

                                // Console.WriteLine("Player's Arena ID: " + player.Arena.Id);

                                var playerEmbed = new DiscordEmbedBuilder
                                {
                                    Title = $"{player.Name}",
                                    Url = $"https://clashapi.net/player/{player.Tag.TrimStart('#')}",
                                    Description = $"{description}",
                                    ThumbnailUrl = $"https://clashapi.net/assets/arenas/arena{arenaId}.png",
                                    Footer = new DiscordEmbedBuilder.EmbedFooter
                                    {
                                        IconUrl = "https://www.freeiconspng.com/uploads/high-resolution-clash-royale-png-icon-21.png",
                                        Text = player.Name + " - via ClashAPI"
                                    },
                                    Color = DiscordColor.Magenta
                                };

                                playerEmbed.AddField("Trophies", $"{player.Trophies.ToString("N0")} / {player.BestTrophies.ToString("N0")} PB", true);
                                playerEmbed.AddField($"{player.Arena.Name}", $"{player.Arena.Name}", true);
                                playerEmbed.AddField("Rank", String.Format("{0}", "Unknown"), true);

                                var playerDetailsEmbed = new DiscordEmbedBuilder
                                {
                                    Color = DiscordColor.Magenta
                                };

                                playerDetailsEmbed.AddField("Ladder wins / losses",
                                    $"{player.Wins.ToString("N0")} / {player.Losses.ToString("N0")} <:battle:665965321408217119>", true);
                                playerDetailsEmbed.AddField("Ladder win percentage",
                                    $"{(((float)player.Wins / (player.BattleCount - draws))).ToString("P")} <:battle:665965321408217119>", true);
                                playerDetailsEmbed.AddField("Total games",
                                    $"{player.BattleCount.ToString("N0")} <:battle:665965321408217119>\n", true);
                                playerDetailsEmbed.AddField("Challenge max wins",
                                    $"{player.ChallengeMaxWins.ToString("N0")} <:tournament:665968060682731570>", true);
                                playerDetailsEmbed.AddField("Challenge cards won",
                                    $"{player.ChallengeCardsWon.ToString("N0")} <:tournament:665968060682731570>", true);
                                playerDetailsEmbed.AddField("Three crown wins",
                                    $"{player.ThreeCrownWins.ToString("N0")} <:crownblue:665968438879059989>\n", true);
                                playerDetailsEmbed.AddField("Tourney cards won",
                                    $"{player.TournamentCardsWon.ToString("N0")} <:tournament:665968060682731570>", true);
                                playerDetailsEmbed.AddField("Tourney games",
                                    $"{player.TournamentBattleCount.ToString("N0")} <:tournament:665968060682731570>", true);
                                playerDetailsEmbed.AddField("Tourney cards / game",
                                    $"{(player.TournamentCardsWon / player.TournamentBattleCount).ToString("N")} <:tournament:665968060682731570>\n", true);
                                playerDetailsEmbed.AddField("Cards found",
                                    $"{player.Cards.Length.ToString("N0")} <:cards:665968792534253579>", true);
                                playerDetailsEmbed.AddField("Total donations",
                                    $"{player.TotalDonations.ToString("N0")} <:cards:665968792534253579>", true);
                                playerDetailsEmbed.AddField("Level",
                                    $"{player.ExpLevel.ToString("N0")} <:experience:665968792664277012>\n", true);
                                playerDetailsEmbed.AddField("Favorite card",
                                    $"{player.CurrentFavouriteCard.Name}");

                                await e.Channel.SendMessageAsync(embed: playerEmbed).ConfigureAwait(false);
                                await e.Channel.SendMessageAsync(embed: playerDetailsEmbed).ConfigureAwait(false);
                            }

                            if (words[0] == "!clan" && e.Message.Content.Trim().Length > 5)
                            {
                                // var message = e.Message.Content.Split(' ');
                                // TODO
                            }
                        }

                        await FilterMessageAsync(e, guild, words, blacklistedWords);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private async Task OnGuildMemberAdded(GuildMemberAddEventArgs e)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                    var guild = await guildService.GetByGuildIdAsync(e.Guild.Id.ToString());

                    if (guild.TemporaryBans.Select(b => b.MemberId).ToList().Contains(e.Member.Id))
                    {
                        var ban = guild.TemporaryBans.First(b => b.MemberId == e.Member.Id);

                        // For some reason, this is needed in order the ban to occur
                        Console.WriteLine(ban);

                        if (ban.ExpiresAt > DateTimeOffset.Now)
                        {
                            await e.Guild.RemoveMemberAsync(e.Member, $"User has been temporarily banned until {ban.ExpiresAt}.");
                            await e.Member.SendMessageAsync(
                                $"You have been temporarily banned from guild {e.Guild.Name} until {ban.ExpiresAt}.");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public int? GetPing()
        {
            return _bot.Client.Ping;
        }

        public bool GetIfBotOnline()
        {
            return _bot.Client.CurrentUser.Presence.Status.Equals(UserStatus.Online);
        }

        public async Task<IReadOnlyList<DiscordChannel>> GetChannelsAsync(string guildId)
        {
            var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));

            return await guild.GetChannelsAsync();
        }

        public async Task<bool> EditGuildAsync(GuildDto guildDto, User user)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                    var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                    var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                    var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildDto.Id));
                    var initiator =
                        await userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                    var member = await GetMemberAsync(guild.Id.ToString(), ulong.Parse(initiator.UserId));

                    var dbGuild = await guildService.GetByGuildIdAsync(guild.Id.ToString());

                    var hasPermission = await CheckIfHasPermissionAsync(member, guild, dbGuild);

                    if (!hasPermission)
                    {
                        return false;
                    }

                    // TODO: For some reason, this is needed or the deleted managers will not be removed
                    Console.WriteLine("Managers: " + dbGuild.Managers);

                    dbGuild.Managers = new List<User>();

                    foreach (var manager in guildDto.Managers)
                    {
                        var dbUser =
                            await userService.GetByUsernameAndDiscriminatorAsync(manager.Username,
                                manager.Discriminator);
                        if (dbUser != null)
                        {
                            dbGuild.Managers.Add(dbUser);
                        }
                    }

                    await guildService.SaveAllAsync();

                    await guild.ModifyAsync(
                        name: guildDto.Name,
                        region: guildDto.Region,
                        default_message_notifications: guildDto.DefaultMessageNotifications,
                        verification_level: guildDto.VerificationLevel,
                        explicit_content_filter: guildDto.ExplicitContentFilter
                        );
                    await logService.AddAsync(
                        $"MODIFIED_GUILD",
                        ActionType.UPDATE,
                        initiator,
                        guildDto.Id
                    );

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<DiscordGuild> GetGuildAsync(string guildId)
        {
            return await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
        }

        public async Task<DiscordMember> GetMemberAsync(string guildId, ulong userId)
        {
            var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));

            return await guild.GetMemberAsync(userId);
        }

        public async Task<List<DiscordRole>> GetMemberRolesAsync(string guildId, ulong userId)
        {
            var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
            var member = await GetMemberAsync(guildId, userId);

            return member.Roles.ToList();
        }

        public async Task<IReadOnlyList<DiscordMember>> GetMembersAsync(string guildId)
        {
            var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));

            return await guild.GetAllMembersAsync();
        }

        public async Task<IReadOnlyList<DiscordMember>> GetMembersInRoleAsync(string guildId, ulong roleId)
        {
            var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
            var role = guild.GetRole(roleId);

            return guild.Members.Where(m => m.Roles.Contains(role)).ToList();
        }

        public async Task<DiscordRole> GetRoleAsync(string guildId, ulong roleId)
        {
            var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));

            return guild.GetRole(roleId);
        }

        public async Task<IReadOnlyList<DiscordRole>> GetRolesAsync(string guildId)
        {
            var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));

            return guild.Roles.ToList();
        }

        public async Task<bool> GrantRoleAsync(string guildId, ulong userId, ulong roleId, User user)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
                var role = guild.Roles.First(r => r.Id == roleId);
                var member = guild.Members.First(m => m.Id == userId);
                var initiator =
                    await userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                var initiatorMember = await GetMemberAsync(guildId, ulong.Parse(initiator.UserId));

                var dbGuild = await guildService.GetByGuildIdAsync(guildId);

                var hasPermission = await CheckIfHasPermissionAsync(initiatorMember, guild, dbGuild);

                if (!hasPermission)
                {
                    return false;
                }

                await guild.GrantRoleAsync(member, role);
                await logService.AddAsync(
                    $"Granted role: {role.Name} ID :{role.Id} to member: {member.Username}#{member.Discriminator}",
                    ActionType.UPDATE,
                    initiator,
                    guildId
                );

                return true;
            }
        }

        public async Task<bool> RevokeRolesAsync(
            string guildId,
            ulong userId,
            IList<DiscordRole> discordRoles,
            User user
            )
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
                var member = guild.Members.First(m => m.Id == userId);
                var initiator =
                    await userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                var initiatorMember = await GetMemberAsync(guildId, ulong.Parse(initiator.UserId));

                var dbGuild = await guildService.GetByGuildIdAsync(guildId);

                var hasPermission = await CheckIfHasPermissionAsync(initiatorMember, guild, dbGuild);

                if (!hasPermission)
                {
                    return false;
                }

                foreach (var discordRole in discordRoles)
                {
                    var role = guild.Roles.First(r => r.Position == discordRole.Position);
                    await member.RevokeRoleAsync(role);
                    await logService.AddAsync(
                        $"Revoked role: {role.Name} ID: {role.Id} from member: {member.Username}#{member.Discriminator}",
                        ActionType.UPDATE,
                        initiator,
                        guildId
                    );
                }

                return true;
            }
        }

        public async Task<bool> RevokeRoleAsync(
            string guildId,
            ulong userId,
            ulong roleId,
            User initiator,
            string reason = "No reason specified"
            )
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
                var member = guild.Members.First(m => m.Id == userId);
                var role = guild.Roles.First(r => r.Id == roleId);
                var initiatorMember = await GetMemberAsync(guildId, ulong.Parse(initiator.UserId));

                var dbGuild = await guildService.GetByGuildIdAsync(guildId);

                var hasPermission = await CheckIfHasPermissionAsync(initiatorMember, guild, dbGuild);

                if (!hasPermission)
                {
                    return false;
                }

                await guild.RevokeRoleAsync(member, role, reason);
                await logService.AddAsync(
                    $"Revoked role: {role.Name} ID: {role.Id} from member: {member.Username}#{member.Discriminator} reason: {reason}",
                    ActionType.UPDATE,
                    initiator,
                    guildId
                );

                return true;
            }
        }

        public async Task<bool> RemoveMemberAsync(User user, string guildId, ulong userId,
            string reason = "No reason specified", bool notifyUser = false, bool notifyAnonymously = false, bool includeReason = true)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
                var discordMember = guild.Members.First(m => m.Id == userId);
                var initiator = await userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                var initiatorMember = await GetMemberAsync(guildId, ulong.Parse(initiator.UserId));

                var dbGuild = await guildService.GetByGuildIdAsync(guildId);

                var hasPermission = await CheckIfHasPermissionAsync(initiatorMember, guild, dbGuild);

                if (!hasPermission)
                {
                    return false;
                }

                if (notifyUser)
                {
                    var staff = notifyAnonymously ? "a staff member" : user.UserName;
                    var message = new StringBuilder();
                    message.Append($"You have been removed from the {guild.Name} guild by {staff}.");

                    if (includeReason)
                    {
                        message.Append($"Reason: {reason}.");
                    }

                    await discordMember.SendMessageAsync(message.ToString());
                }

                await guild.RemoveMemberAsync(guild.Members.First(m => m.Id == userId), reason);
                await logService.AddAsync(
                    $"Kicked member: {discordMember.Username}#{discordMember.Discriminator}",
                    ActionType.UPDATE,
                    initiator,
                    guildId
                );

                return true;
            }
        }

        public async Task<bool> BanMemberAsync(
            User user, string guildId, ulong userId,
            string reason = "No reason specified", bool notifyUser = false,
            bool notifyAnonymously = false, bool includeReason = true
            )
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
                var discordMember = guild.Members.First(m => m.Id == userId);
                var initiator =
                    await userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                var initiatorMember = await GetMemberAsync(guildId, ulong.Parse(initiator.UserId));

                var dbGuild = await guildService.GetByGuildIdAsync(guildId);

                var hasPermission = await CheckIfHasPermissionAsync(initiatorMember, guild, dbGuild);

                if (!hasPermission)
                {
                    return false;
                }

                if (notifyUser)
                {
                    var staff = notifyAnonymously ? "a staff member" : user.UserName;
                    var message = new StringBuilder();
                    message.Append($"You have been banned from the {guild.Name} guild by {staff}.");

                    if (includeReason)
                    {
                        message.Append($"Reason: {reason}.");
                    }

                    try
                    {
                        await discordMember.SendMessageAsync(message.ToString());
                    }
                    catch (UnauthorizedException e)
                    {
                        // TODO: Handle could not send DM error
                    }
                }

                await guild.BanMemberAsync(guild.Members.First(m => m.Id == userId), 0, reason);
                await logService.AddAsync(
                    $"Banned member: {discordMember.Username}#{discordMember.Discriminator}",
                    ActionType.UPDATE,
                    initiator,
                    guildId
                );

                return true;
            }
        }

        public async Task<bool> DeleteRoleAsync(string guildId, ulong id, User user)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
                var initiatorMember = await GetMemberAsync(guildId, ulong.Parse(user.UserId));

                var dbGuild = await guildService.GetByGuildIdAsync(guildId);

                var hasPermission = await CheckIfHasPermissionAsync(initiatorMember, guild, dbGuild);

                if (!hasPermission)
                {
                    return false;
                }

                if (!await CheckPermissionsAsync(guild, user))
                {
                    return false;
                }

                var role = guild.Roles.First(r => r.Id == id);
                var initiator = await userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);

                // await guild.DeleteRoleAsync(role);
                await logService.AddAsync(
                    $"Deleted role: {role.Name} ID: {role.Id}",
                    ActionType.DELETE,
                    initiator,
                    guildId
                );

                return true;
            }
        }

        public async Task<bool> ConnectAsync()
        {
            var connections = await _bot.Client.GetConnectionsAsync();

            if (connections.Count > 0)
            {
                return false;
            }

            await _bot.Client.ConnectAsync();
            return true;
        }

        public async Task<bool> DisconnectAsync()
        {
            await _bot.Client.DisconnectAsync();
            return true;
        }

        public async Task<bool> ReconnectAsync()
        {
            await _bot.Client.ReconnectAsync();
            return true;
        }

        public async Task<bool> HandleEditMemberAsync(string guildId, EditMemberDto editMemberDto, User user)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
                var memberId = ulong.Parse(editMemberDto.MemberId);
                var member = await guild.GetMemberAsync(memberId);
                var initiator
                    = await userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                var initiatorMember = await guild.GetMemberAsync(ulong.Parse(initiator.UserId));

                var dbGuild = await guildService.GetByGuildIdAsync(guildId);

                var hasPermission = await CheckIfHasPermissionAsync(initiatorMember, guild, dbGuild);

                if (!hasPermission)
                {
                    return false;
                }

                if (editMemberDto.DisplayName != member.DisplayName)
                {
                    if (!member.IsOwner)
                    {
                        await logService.AddAsync(
                            $"Changed member nick from: {member.DisplayName} to: {editMemberDto.DisplayName}",
                            ActionType.UPDATE,
                            initiator,
                            guildId
                        );
                        await SetMemberDisplayNameAsync(guildId, memberId, editMemberDto.DisplayName);
                    }
                }

                if (editMemberDto.IsDeafened != member.IsDeafened)
                {
                    await SetMemberDeafAsync(guildId, memberId, editMemberDto.IsDeafened);
                    await logService.AddAsync(
                        $"Deafened user: {member.DisplayName}#{member.Discriminator}",
                        ActionType.UPDATE,
                        initiator,
                        guildId
                    );
                }

                if (editMemberDto.IsMuted != member.IsMuted)
                {
                    await SetMemberMuteAsync(guildId, memberId, editMemberDto.IsMuted);
                    await logService.AddAsync(
                        $"Muted user: {member.DisplayName}#{member.Discriminator}",
                        ActionType.UPDATE,
                        initiator,
                        guildId
                    );
                }

                await logService.AddAsync(
                    $"Edited user: {member.DisplayName}#{member.Discriminator}",
                    ActionType.UPDATE,
                    initiator,
                    guildId
                );

                return true;
            }
        }

        private async Task<bool> SetMemberDisplayNameAsync(string guildId, ulong userId, string nickname, string reason = null)
        {
            var member = await GetMemberAsync(guildId, userId);
            await member.ModifyAsync(nickname: nickname, reason: reason);

            return true;
        }

        private async Task<bool> SetMemberDeafAsync(string guildId, ulong userId, bool isDeafened, string reason = null)
        {
            var member = await GetMemberAsync(guildId, userId);
            await member.SetDeafAsync(isDeafened, reason);

            return true;
        }

        private async Task<bool> SetMemberMuteAsync(string guildId, ulong userId, bool isMuted, string reason = null)
        {
            var member = await GetMemberAsync(guildId, userId);
            await member.SetMuteAsync(isMuted, reason);

            return true;
        }

        public string GetClientUserId()
        {
            return _bot.Client.CurrentUser.Id.ToString();
        }

        public bool GetIfBotIsMemberOfGuild(string guildId)
        {
            return _bot.Client.Guilds.Keys.Contains(ulong.Parse(guildId));
        }

        public int? GetBotPing()
        {
            return _bot.Client.Ping;
        }

        public async Task<IReadOnlyList<DiscordVoiceRegion>> GetRegionsAsync()
        {
            return await _bot.Client.ListRegionsAsync();
        }

        public async Task<IReadOnlyList<DiscordBan>> GetBansAsync(string guildId, User user)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();

                var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));

                var initiatorMember = await guild.GetMemberAsync(ulong.Parse(user.UserId));

                var dbGuild = await guildService.GetByGuildIdAsync(guildId);

                var hasPermission = await CheckIfHasPermissionAsync(initiatorMember, guild, dbGuild);

                if (!hasPermission)
                {
                    return null;
                }

                return await guild.GetBansAsync();
            }
        }

        public async Task<bool> UnbanMembersAsync(string guildId, UnbanMembersDto unbanMembersDto, User user)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                    var logService = scope.ServiceProvider.GetRequiredService<ILogService>();

                    var guild = await _bot.Client.GetGuildAsync(ulong.Parse(guildId));
                    var bans = await GetBansAsync(guildId, user);

                    var initiatorMember = await guild.GetMemberAsync(ulong.Parse(user.UserId));

                    var dbGuild = await guildService.GetByGuildIdAsync(guildId);

                    var hasPermission = await CheckIfHasPermissionAsync(initiatorMember, guild, dbGuild);

                    if (!hasPermission)
                    {
                        return false;
                    }

                    foreach (var unbanRequest in unbanMembersDto.Members)
                    {
                        var ban = bans.FirstOrDefault(b =>
                            b.User.Username == unbanRequest.Username && b.User.Discriminator == unbanRequest.Discriminator);
                        await guild.UnbanMemberAsync(ban.User.Id);
                        await logService.AddAsync(
                            $"Unbanned user {ban.User.Username}#{ban.User.Discriminator}",
                            ActionType.UPDATE,
                            user,
                            guildId
                        );
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}