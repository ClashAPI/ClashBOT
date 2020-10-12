using backend.Data;
using backend.DTOs;
using backend.Helpers;
using backend.Models;
using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DiscordChannel = backend.Models.DiscordChannel;
using DiscordRole = backend.Models.DiscordRole;

namespace backend.Services
{
    public class PluginService : IPluginService
    {
        private readonly IPluginRepository _pluginRepository;
        private readonly IGuildService _guildService;
        private readonly IUserService _userService;
        private readonly ILogService _logService;
        private readonly IBotService _botService;

        public PluginService(IPluginRepository pluginRepository, IGuildService guildService, 
            IUserService userService, ILogService logService, IBotService botService)
        {
            _pluginRepository = pluginRepository;
            _guildService = guildService;
            _userService = userService;
            _logService = logService;
            _botService = botService;
        }

        private async Task<bool> CheckIfHasPermissionAsync(DiscordUser discordUser, DiscordGuild discordGuild,
            Guild dbGuild)
        {
            var hasPermission = false;
            var invoker = await _botService.GetMemberAsync(dbGuild.GuildId, discordUser.Id);

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

        public async Task<bool> UpdateAutomodPluginAsync(
            string guildId,
            AutoModPluginDto autoModPluginDto,
            User user)
        {
            try
            {
                var guild = await _guildService.GetByGuildIdAsync(guildId);
                var discordGuild = await _botService.GetGuildAsync(guildId);
                var initiator =
                    await _userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                var member = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));
                var hasPermission = await CheckIfHasPermissionAsync(member, discordGuild, guild);

                if (!hasPermission)
                {
                    return false;
                }

                foreach (var word in guild.AutoModPlugin.BadWordsSettings.BadWords
                    .Select(w => w.Word)
                    .ToList())
                {
                    if (!autoModPluginDto.BadWordsSettings.BadWords.Select(w => w.Word).ToList().Contains(word))
                    {
                        guild.AutoModPlugin.BadWordsSettings.BadWords.Remove(
                            guild.AutoModPlugin.BadWordsSettings.BadWords.FirstOrDefault(w => w.Word == word));
                    }
                }

                foreach (var command in guild.AutoModPlugin.ModeratorCommands)
                {
                    var dtoCommand = autoModPluginDto.ModeratorCommands.FirstOrDefault(c => c.Id == command.Id);
                    if (command.IsEnabled != dtoCommand.IsEnabled)
                    {
                        command.IsEnabled = dtoCommand.IsEnabled;
                    }
                }

                foreach (var word in autoModPluginDto.BadWordsSettings.BadWords)
                {
                    if (!guild.AutoModPlugin.BadWordsSettings.BadWords
                        .Select(w => w.Word)
                        .ToList().Contains(word.Word))
                    {
                        guild.AutoModPlugin.BadWordsSettings.BadWords.Add(new BadWord
                        {
                            Guild = guild,
                            Word = word.Word
                        });
                    }
                }

                var guildRoles = new List<ModeratorRole>();

                foreach (var guildRole in guild.AutoModPlugin.ModeratorRoles)
                {
                    guildRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.ModeratorRoles)
                {
                    guildRoles.Add(new ModeratorRole
                    {
                        RoleId = role.RoleId,
                        AutoModPlugin = guild.AutoModPlugin
                    });
                }

                foreach (var role in guild.AutoModPlugin.ModeratorRoles)
                {
                    if (!autoModPluginDto.ModeratorRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveModeratorRole(role);
                    }
                }

                guild.AutoModPlugin.ModeratorRoles = guildRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var badWordsSettingsEnabledRoles = new List<DiscordRole>();

                foreach (var guildRole in guild.AutoModPlugin.BadWordsSettings.AllowedRoles)
                {
                    badWordsSettingsEnabledRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.BadWordsSettings.AllowedRoles)
                {
                    badWordsSettingsEnabledRoles.Add(new DiscordRole
                    {
                        RoleId = role.RoleId
                    });
                }

                foreach (var role in guild.AutoModPlugin.BadWordsSettings.AllowedRoles)
                {
                    if (!autoModPluginDto.BadWordsSettings.AllowedRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveAllowedRole(role);
                    }
                }

                guild.AutoModPlugin.BadWordsSettings.AllowedRoles =
                    badWordsSettingsEnabledRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var badWordsSettingsIgnoredChannels = new List<DiscordChannel>();

                foreach (var guildChannel in guild.AutoModPlugin.BadWordsSettings.IgnoredChannels)
                {
                    badWordsSettingsIgnoredChannels.Add(guildChannel);
                }

                foreach (var channel in autoModPluginDto.BadWordsSettings.IgnoredChannels)
                {
                    badWordsSettingsIgnoredChannels.Add(new DiscordChannel
                    {
                        ChannelId = channel.ChannelId
                    });
                }

                foreach (var channel in guild.AutoModPlugin.BadWordsSettings.IgnoredChannels)
                {
                    if (!autoModPluginDto.BadWordsSettings.IgnoredChannels.Select(c => c.ChannelId).ToList().Contains(channel.ChannelId))
                    {
                        var dbChannel =
                            guild.AutoModPlugin.BadWordsSettings.IgnoredChannels.FirstOrDefault(c =>
                                c.ChannelId == channel.ChannelId);
                        guild.AutoModPlugin.BadWordsSettings.IgnoredChannels.Remove(dbChannel);
                    }
                }

                guild.AutoModPlugin.BadWordsSettings.IgnoredChannels =
                    badWordsSettingsIgnoredChannels.GroupBy(c => c.ChannelId).Select(c => c.First()).ToList();

                var excessiveCapsSettingsEnabledRoles = new List<DiscordRole>();

                foreach (var guildRole in guild.AutoModPlugin.ExcessiveCapsSettings.AllowedRoles)
                {
                    excessiveCapsSettingsEnabledRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.ExcessiveCapsSettings.AllowedRoles)
                {
                    excessiveCapsSettingsEnabledRoles.Add(new DiscordRole
                    {
                        RoleId = role.RoleId
                    });
                }

                foreach (var role in guild.AutoModPlugin.ExcessiveCapsSettings.AllowedRoles)
                {
                    if (!autoModPluginDto.ExcessiveCapsSettings.AllowedRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveAllowedRole(role);
                    }
                }

                guild.AutoModPlugin.BadWordsSettings.AllowedRoles =
                    excessiveCapsSettingsEnabledRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var excessiveCapsSettingsIgnoredChannels = new List<DiscordChannel>();

                foreach (var guildChannel in guild.AutoModPlugin.ExcessiveCapsSettings.IgnoredChannels)
                {
                    excessiveCapsSettingsIgnoredChannels.Add(guildChannel);
                }

                foreach (var channel in autoModPluginDto.ExcessiveCapsSettings.IgnoredChannels)
                {
                    excessiveCapsSettingsIgnoredChannels.Add(new DiscordChannel
                    {
                        ChannelId = channel.ChannelId
                    });
                }

                foreach (var channel in guild.AutoModPlugin.ExcessiveCapsSettings.IgnoredChannels)
                {
                    if (!autoModPluginDto.ExcessiveCapsSettings.IgnoredChannels.Select(c => c.ChannelId).ToList().Contains(channel.ChannelId))
                    {
                        var dbChannel =
                            guild.AutoModPlugin.ExcessiveCapsSettings.IgnoredChannels.FirstOrDefault(c =>
                                c.ChannelId == channel.ChannelId);
                        guild.AutoModPlugin.ExcessiveCapsSettings.IgnoredChannels.Remove(dbChannel);
                    }
                }

                guild.AutoModPlugin.ExcessiveCapsSettings.IgnoredChannels =
                    excessiveCapsSettingsIgnoredChannels.GroupBy(c => c.ChannelId).Select(c => c.First()).ToList();

                var repeatedTextEnabledRoles = new List<DiscordRole>();

                foreach (var guildRole in guild.AutoModPlugin.RepeatedTextSettings.AllowedRoles)
                {
                    repeatedTextEnabledRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.RepeatedTextSettings.AllowedRoles)
                {
                    repeatedTextEnabledRoles.Add(new DiscordRole
                    {
                        RoleId = role.RoleId
                    });
                }

                foreach (var role in guild.AutoModPlugin.RepeatedTextSettings.AllowedRoles)
                {
                    if (!autoModPluginDto.RepeatedTextSettings.AllowedRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveAllowedRole(role);
                    }
                }

                guild.AutoModPlugin.RepeatedTextSettings.AllowedRoles =
                    repeatedTextEnabledRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var repeatedTextIgnoredChannels = new List<DiscordChannel>();

                foreach (var guildChannel in guild.AutoModPlugin.RepeatedTextSettings.IgnoredChannels)
                {
                    repeatedTextIgnoredChannels.Add(guildChannel);
                }

                foreach (var channel in autoModPluginDto.RepeatedTextSettings.IgnoredChannels)
                {
                    repeatedTextIgnoredChannels.Add(new DiscordChannel
                    {
                        ChannelId = channel.ChannelId
                    });
                }

                foreach (var channel in guild.AutoModPlugin.RepeatedTextSettings.IgnoredChannels)
                {
                    if (!autoModPluginDto.RepeatedTextSettings.IgnoredChannels.Select(c => c.ChannelId).ToList().Contains(channel.ChannelId))
                    {
                        var dbChannel =
                            guild.AutoModPlugin.RepeatedTextSettings.IgnoredChannels.FirstOrDefault(c =>
                                c.ChannelId == channel.ChannelId);
                        guild.AutoModPlugin.RepeatedTextSettings.IgnoredChannels.Remove(dbChannel);
                    }
                }

                guild.AutoModPlugin.RepeatedTextSettings.IgnoredChannels =
                    repeatedTextIgnoredChannels.GroupBy(c => c.ChannelId).Select(c => c.First()).ToList();

                var excessiveEmojisEnabledRoles = new List<DiscordRole>();

                foreach (var guildRole in guild.AutoModPlugin.ExcessiveEmojisSettings.AllowedRoles)
                {
                    excessiveEmojisEnabledRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.ExcessiveEmojisSettings.AllowedRoles)
                {
                    excessiveEmojisEnabledRoles.Add(new DiscordRole
                    {
                        RoleId = role.RoleId
                    });
                }

                foreach (var role in guild.AutoModPlugin.ExcessiveEmojisSettings.AllowedRoles)
                {
                    if (!autoModPluginDto.ExcessiveEmojisSettings.AllowedRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveAllowedRole(role);
                    }
                }

                guild.AutoModPlugin.ExcessiveEmojisSettings.AllowedRoles =
                    excessiveEmojisEnabledRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var excessiveEmojisIgnoredChannels = new List<DiscordChannel>();

                foreach (var guildChannel in guild.AutoModPlugin.ExcessiveEmojisSettings.IgnoredChannels)
                {
                    excessiveEmojisIgnoredChannels.Add(guildChannel);
                }

                foreach (var channel in autoModPluginDto.ExcessiveEmojisSettings.IgnoredChannels)
                {
                    excessiveEmojisIgnoredChannels.Add(new DiscordChannel
                    {
                        ChannelId = channel.ChannelId
                    });
                }

                foreach (var channel in guild.AutoModPlugin.ExcessiveEmojisSettings.IgnoredChannels)
                {
                    if (!autoModPluginDto.ExcessiveEmojisSettings.IgnoredChannels.Select(c => c.ChannelId).ToList().Contains(channel.ChannelId))
                    {
                        var dbChannel =
                            guild.AutoModPlugin.ExcessiveEmojisSettings.IgnoredChannels.FirstOrDefault(c =>
                                c.ChannelId == channel.ChannelId);
                        guild.AutoModPlugin.ExcessiveEmojisSettings.IgnoredChannels.Remove(dbChannel);
                    }
                }

                guild.AutoModPlugin.ExcessiveEmojisSettings.IgnoredChannels =
                    excessiveEmojisIgnoredChannels.GroupBy(c => c.ChannelId).Select(c => c.First()).ToList();

                var excessiveSpoilersEnabledRoles = new List<DiscordRole>();

                foreach (var guildRole in guild.AutoModPlugin.ExcessiveSpoilersSettings.AllowedRoles)
                {
                    excessiveSpoilersEnabledRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.ExcessiveSpoilersSettings.AllowedRoles)
                {
                    excessiveSpoilersEnabledRoles.Add(new DiscordRole
                    {
                        RoleId = role.RoleId
                    });
                }

                foreach (var role in guild.AutoModPlugin.ExcessiveSpoilersSettings.AllowedRoles)
                {
                    if (!autoModPluginDto.ExcessiveSpoilersSettings.AllowedRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveAllowedRole(role);
                    }
                }

                guild.AutoModPlugin.ExcessiveSpoilersSettings.AllowedRoles =
                    excessiveSpoilersEnabledRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var excessiveSpoilersIgnoredChannels = new List<DiscordChannel>();

                foreach (var guildChannel in guild.AutoModPlugin.ExcessiveSpoilersSettings.IgnoredChannels)
                {
                    excessiveSpoilersIgnoredChannels.Add(guildChannel);
                }

                foreach (var channel in autoModPluginDto.ExcessiveSpoilersSettings.IgnoredChannels)
                {
                    excessiveSpoilersIgnoredChannels.Add(new DiscordChannel
                    {
                        ChannelId = channel.ChannelId
                    });
                }

                foreach (var channel in guild.AutoModPlugin.ExcessiveSpoilersSettings.IgnoredChannels)
                {
                    if (!autoModPluginDto.ExcessiveSpoilersSettings.IgnoredChannels.Select(c => c.ChannelId).ToList().Contains(channel.ChannelId))
                    {
                        var dbChannel =
                            guild.AutoModPlugin.ExcessiveSpoilersSettings.IgnoredChannels.FirstOrDefault(c =>
                                c.ChannelId == channel.ChannelId);
                        guild.AutoModPlugin.ExcessiveSpoilersSettings.IgnoredChannels.Remove(dbChannel);
                    }
                }

                guild.AutoModPlugin.ExcessiveSpoilersSettings.IgnoredChannels =
                    excessiveSpoilersIgnoredChannels.GroupBy(c => c.ChannelId).Select(c => c.First()).ToList();

                var externalLinksEnabledRoles = new List<DiscordRole>();

                foreach (var guildRole in guild.AutoModPlugin.ExternalLinksSettings.AllowedRoles)
                {
                    externalLinksEnabledRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.ExternalLinksSettings.AllowedRoles)
                {
                    externalLinksEnabledRoles.Add(new DiscordRole
                    {
                        RoleId = role.RoleId
                    });
                }

                foreach (var role in guild.AutoModPlugin.ExternalLinksSettings.AllowedRoles)
                {
                    if (!autoModPluginDto.ExternalLinksSettings.AllowedRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveAllowedRole(role);
                    }
                }

                guild.AutoModPlugin.ExternalLinksSettings.AllowedRoles =
                    externalLinksEnabledRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var externalLinksIgnoredChannels = new List<DiscordChannel>();

                foreach (var guildChannel in guild.AutoModPlugin.ExternalLinksSettings.IgnoredChannels)
                {
                    externalLinksIgnoredChannels.Add(guildChannel);
                }

                foreach (var channel in autoModPluginDto.ExternalLinksSettings.IgnoredChannels)
                {
                    externalLinksIgnoredChannels.Add(new DiscordChannel
                    {
                        ChannelId = channel.ChannelId
                    });
                }

                foreach (var channel in guild.AutoModPlugin.ExternalLinksSettings.IgnoredChannels)
                {
                    if (!autoModPluginDto.ExternalLinksSettings.IgnoredChannels.Select(c => c.ChannelId).ToList().Contains(channel.ChannelId))
                    {
                        var dbChannel =
                            guild.AutoModPlugin.ExternalLinksSettings.IgnoredChannels.FirstOrDefault(c =>
                                c.ChannelId == channel.ChannelId);
                        guild.AutoModPlugin.ExternalLinksSettings.IgnoredChannels.Remove(dbChannel);
                    }
                }

                guild.AutoModPlugin.ExternalLinksSettings.IgnoredChannels =
                    externalLinksIgnoredChannels.GroupBy(c => c.ChannelId).Select(c => c.First()).ToList();

                var serverInvitesEnabledRoles = new List<DiscordRole>();

                foreach (var guildRole in guild.AutoModPlugin.ServerInvitesSettings.AllowedRoles)
                {
                    serverInvitesEnabledRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.ServerInvitesSettings.AllowedRoles)
                {
                    serverInvitesEnabledRoles.Add(new DiscordRole
                    {
                        RoleId = role.RoleId
                    });
                }

                foreach (var role in guild.AutoModPlugin.ServerInvitesSettings.AllowedRoles)
                {
                    if (!autoModPluginDto.ServerInvitesSettings.AllowedRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveAllowedRole(role);
                    }
                }

                guild.AutoModPlugin.ServerInvitesSettings.AllowedRoles =
                    serverInvitesEnabledRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var serverInvitesIgnoredChannels = new List<DiscordChannel>();

                foreach (var guildChannel in guild.AutoModPlugin.ServerInvitesSettings.IgnoredChannels)
                {
                    serverInvitesIgnoredChannels.Add(guildChannel);
                }

                foreach (var channel in autoModPluginDto.ServerInvitesSettings.IgnoredChannels)
                {
                    serverInvitesIgnoredChannels.Add(new DiscordChannel
                    {
                        ChannelId = channel.ChannelId
                    });
                }

                foreach (var channel in guild.AutoModPlugin.ServerInvitesSettings.IgnoredChannels)
                {
                    if (!autoModPluginDto.ServerInvitesSettings.IgnoredChannels.Select(c => c.ChannelId).ToList().Contains(channel.ChannelId))
                    {
                        var dbChannel =
                            guild.AutoModPlugin.ServerInvitesSettings.IgnoredChannels.FirstOrDefault(c =>
                                c.ChannelId == channel.ChannelId);
                        guild.AutoModPlugin.ServerInvitesSettings.IgnoredChannels.Remove(dbChannel);
                    }
                }

                guild.AutoModPlugin.ServerInvitesSettings.IgnoredChannels =
                    serverInvitesIgnoredChannels.GroupBy(c => c.ChannelId).Select(c => c.First()).ToList();

                var excessiveMentionsEnabledRoles = new List<DiscordRole>();

                foreach (var guildRole in guild.AutoModPlugin.ExcessiveMentionsSettings.AllowedRoles)
                {
                    excessiveMentionsEnabledRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.ExcessiveMentionsSettings.AllowedRoles)
                {
                    excessiveMentionsEnabledRoles.Add(new DiscordRole
                    {
                        RoleId = role.RoleId
                    });
                }

                foreach (var role in guild.AutoModPlugin.ExcessiveMentionsSettings.AllowedRoles)
                {
                    if (!autoModPluginDto.ExcessiveMentionsSettings.AllowedRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveAllowedRole(role);
                    }
                }

                guild.AutoModPlugin.ExcessiveMentionsSettings.AllowedRoles =
                    excessiveMentionsEnabledRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var excessiveMentionsIgnoredChannels = new List<DiscordChannel>();

                foreach (var guildChannel in guild.AutoModPlugin.ExcessiveMentionsSettings.IgnoredChannels)
                {
                    excessiveMentionsIgnoredChannels.Add(guildChannel);
                }

                foreach (var channel in autoModPluginDto.ExcessiveMentionsSettings.IgnoredChannels)
                {
                    excessiveMentionsIgnoredChannels.Add(new DiscordChannel
                    {
                        ChannelId = channel.ChannelId
                    });
                }

                foreach (var channel in guild.AutoModPlugin.ExcessiveMentionsSettings.IgnoredChannels)
                {
                    if (!autoModPluginDto.ExcessiveMentionsSettings.IgnoredChannels.Select(c => c.ChannelId).ToList().Contains(channel.ChannelId))
                    {
                        var dbChannel =
                            guild.AutoModPlugin.ExcessiveMentionsSettings.IgnoredChannels.FirstOrDefault(c =>
                                c.ChannelId == channel.ChannelId);
                        guild.AutoModPlugin.ExcessiveMentionsSettings.IgnoredChannels.Remove(dbChannel);
                    }
                }

                guild.AutoModPlugin.ExcessiveMentionsSettings.IgnoredChannels =
                    excessiveMentionsIgnoredChannels.GroupBy(c => c.ChannelId).Select(c => c.First()).ToList();

                var zalgoEnabledRoles = new List<DiscordRole>();

                foreach (var guildRole in guild.AutoModPlugin.ZalgoSettings.AllowedRoles)
                {
                    zalgoEnabledRoles.Add(guildRole);
                }

                foreach (var role in autoModPluginDto.ZalgoSettings.AllowedRoles)
                {
                    zalgoEnabledRoles.Add(new DiscordRole
                    {
                        RoleId = role.RoleId
                    });
                }

                foreach (var role in guild.AutoModPlugin.ZalgoSettings.AllowedRoles)
                {
                    if (!autoModPluginDto.ZalgoSettings.AllowedRoles.Select(r => r.RoleId).ToList().Contains(role.RoleId))
                    {
                        _pluginRepository.RemoveAllowedRole(role);
                    }
                }

                guild.AutoModPlugin.ZalgoSettings.AllowedRoles =
                    zalgoEnabledRoles.GroupBy(r => r.RoleId).Select(r => r.First()).ToList();

                var zalgoIgnoredChannels = new List<DiscordChannel>();

                foreach (var guildChannel in guild.AutoModPlugin.ZalgoSettings.IgnoredChannels)
                {
                    zalgoIgnoredChannels.Add(guildChannel);
                }

                foreach (var channel in autoModPluginDto.ZalgoSettings.IgnoredChannels)
                {
                    zalgoIgnoredChannels.Add(new DiscordChannel
                    {
                        ChannelId = channel.ChannelId
                    });
                }

                foreach (var channel in guild.AutoModPlugin.ZalgoSettings.IgnoredChannels)
                {
                    if (!autoModPluginDto.ZalgoSettings.IgnoredChannels.Select(c => c.ChannelId).ToList().Contains(channel.ChannelId))
                    {
                        var dbChannel =
                            guild.AutoModPlugin.ZalgoSettings.IgnoredChannels.FirstOrDefault(c =>
                                c.ChannelId == channel.ChannelId);
                        guild.AutoModPlugin.ZalgoSettings.IgnoredChannels.Remove(dbChannel);
                    }
                }

                guild.AutoModPlugin.ZalgoSettings.IgnoredChannels =
                    zalgoIgnoredChannels.GroupBy(c => c.ChannelId).Select(c => c.First()).ToList();

                /*
                foreach (var dtoAutomatedAction in autoModPluginDto.AutomatedActions)
                {
                    guild.AutoModPlugin.AutomatedActions.Add(new AutomatedAction
                    {
                        InfractionsLimit = dtoAutomatedAction.InfractionsLimit,
                        ModerationAction = dtoAutomatedAction.ModerationAction,
                        TimeLimitInSeconds = dtoAutomatedAction.TimeLimitInSeconds
                    });
                }

                foreach (var dbAutomatedAction in guild.AutoModPlugin.AutomatedActions)
                {
                    if (!guild.AutoModPlugin.AutomatedActions.Select(a => a.Id).ToList().Contains(dbAutomatedAction.Id))
                    {
                        var action =
                            guild.AutoModPlugin.AutomatedActions.FirstOrDefault(a => a.Id == dbAutomatedAction.Id);
                        guild.AutoModPlugin.AutomatedActions.Remove(action);
                    }
                }
                */

                if (guild.AutoModPlugin.IsEnabled != autoModPluginDto.IsEnabled)
                {
                    guild.AutoModPlugin.IsEnabled = autoModPluginDto.IsEnabled;
                }

                if (guild.AutoModPlugin.IgnoreBots != autoModPluginDto.IgnoreBots)
                {
                    guild.AutoModPlugin.IgnoreBots = autoModPluginDto.IgnoreBots;
                }

                if (guild.AutoModPlugin.BadWordsSettings.ModerationAction !=
                    autoModPluginDto.BadWordsSettings.ModerationAction)
                {
                    guild.AutoModPlugin.BadWordsSettings.ModerationAction =
                        autoModPluginDto.BadWordsSettings.ModerationAction;
                }

                if (guild.AutoModPlugin.ExcessiveEmojisSettings.ModerationAction !=
                    autoModPluginDto.ExcessiveEmojisSettings.ModerationAction)
                {
                    guild.AutoModPlugin.ExcessiveEmojisSettings.ModerationAction =
                        autoModPluginDto.ExcessiveEmojisSettings.ModerationAction;
                }

                if (guild.AutoModPlugin.ExcessiveCapsSettings.ModerationAction !=
                    autoModPluginDto.ExcessiveCapsSettings.ModerationAction)
                {
                    guild.AutoModPlugin.ExcessiveCapsSettings.ModerationAction =
                        autoModPluginDto.ExcessiveCapsSettings.ModerationAction;
                }

                if (guild.AutoModPlugin.ExcessiveMentionsSettings.ModerationAction !=
                    autoModPluginDto.ExcessiveMentionsSettings.ModerationAction)
                {
                    guild.AutoModPlugin.ExcessiveMentionsSettings.ModerationAction =
                        autoModPluginDto.ExcessiveMentionsSettings.ModerationAction;
                }

                if (guild.AutoModPlugin.ExcessiveSpoilersSettings.ModerationAction !=
                    autoModPluginDto.ExcessiveSpoilersSettings.ModerationAction)
                {
                    guild.AutoModPlugin.ExcessiveSpoilersSettings.ModerationAction =
                        autoModPluginDto.ExcessiveSpoilersSettings.ModerationAction;
                }

                if (guild.AutoModPlugin.ExternalLinksSettings.ModerationAction !=
                    autoModPluginDto.ExternalLinksSettings.ModerationAction)
                {
                    guild.AutoModPlugin.ExternalLinksSettings.ModerationAction =
                        autoModPluginDto.ExternalLinksSettings.ModerationAction;
                }

                if (guild.AutoModPlugin.RepeatedTextSettings.ModerationAction !=
                    autoModPluginDto.RepeatedTextSettings.ModerationAction)
                {
                    guild.AutoModPlugin.RepeatedTextSettings.ModerationAction =
                        autoModPluginDto.RepeatedTextSettings.ModerationAction;
                }

                if (guild.AutoModPlugin.ServerInvitesSettings.ModerationAction !=
                    autoModPluginDto.ServerInvitesSettings.ModerationAction)
                {
                    guild.AutoModPlugin.ServerInvitesSettings.ModerationAction =
                        autoModPluginDto.ServerInvitesSettings.ModerationAction;
                }

                if (guild.AutoModPlugin.ZalgoSettings.ModerationAction !=
                    autoModPluginDto.ZalgoSettings.ModerationAction)
                {
                    guild.AutoModPlugin.ZalgoSettings.ModerationAction =
                        autoModPluginDto.ZalgoSettings.ModerationAction;
                }

                await _logService.AddAsync(
                    $"CHANGED_MODERATOR_PLUGIN_SETTINGS",
                    ActionType.UPDATE,
                    initiator,
                    guildId
                    );
                await _pluginRepository.SaveAllAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateCustomCommandsPluginAsync(string guildId, CustomCommandsPluginDto customCommandPluginDto, User user)
        {
            try
            {
                var guild = await _guildService.GetByGuildIdAsync(guildId);
                var discordGuild = await _botService.GetGuildAsync(guildId);
                var member = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));
                //var initiator = 
                //    await _userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);

                var hasPermission = await CheckIfHasPermissionAsync(member, discordGuild, guild);

                if (!hasPermission)
                {
                    return false;
                }

                // TODO: For some reason, this is needed or the deleted commands will not be removed
                Console.WriteLine("Commands BEFORE modification: " + guild.CustomCommandPlugin.Commands);

                guild.CustomCommandPlugin.Commands = customCommandPluginDto.Commands;

                guild.CustomCommandPlugin.Commands = new List<Command>();
                foreach (var command in customCommandPluginDto.Commands)
                {
                    if (command.Id != null)
                    {
                        command.Id = new Guid();
                    }

                    guild.CustomCommandPlugin.Commands.Add(command);
                }

                // TODO: For some reason, this is needed or the deleted commands will not be removed
                Console.WriteLine("Commands BEFORE modification: " + guild.CustomCommandPlugin.AdvancedCommands);

                guild.CustomCommandPlugin.AdvancedCommands = customCommandPluginDto.AdvancedCommands;

                guild.CustomCommandPlugin.AdvancedCommands = new List<AdvancedCommand>();
                foreach (var command in customCommandPluginDto.AdvancedCommands)
                {
                    if (command.Id != null)
                    {
                        command.Id = new Guid();
                    }

                    guild.CustomCommandPlugin.AdvancedCommands.Add(command);
                }

                if (guild.CustomCommandPlugin.IsEnabled != customCommandPluginDto.IsEnabled)
                {
                    guild.CustomCommandPlugin.IsEnabled = customCommandPluginDto.IsEnabled;
                }

                await _logService.AddAsync(
                    $"CHANGED_CUSTOM_COMMANDS_PLUGIN_SETTINGS",
                    ActionType.UPDATE,
                    user,
                    guildId
                );

                await _pluginRepository.SaveAllAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        
        public async Task<bool> UpdateScheduledMessagesPluginAsync(string guildId, ScheduledMessagesPluginDto scheduledMessagesPluginDto, User user)
        {
            try
            {
                var guild = await _guildService.GetByGuildIdAsync(guildId);
                var discordGuild = await _botService.GetGuildAsync(guildId);
                var member = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));

                var hasPermission = await CheckIfHasPermissionAsync(member, discordGuild, guild);

                if (scheduledMessagesPluginDto.IsEnabled != guild.ScheduledMessagesPlugin.IsEnabled)
                {
                    guild.ScheduledMessagesPlugin.IsEnabled = scheduledMessagesPluginDto.IsEnabled;
                }

                if (!hasPermission)
                {
                    return false;
                }

                var initiator =
                    await _userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);

                // TODO: log
                
                await _pluginRepository.SaveAllAsync();
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> UpdateTwitchPluginAsync(string guildId, TwitchPluginDto twitchPluginDto, User user)
        {
            try
            {
                var guild = await _guildService.GetByGuildIdAsync(guildId);
                var discordGuild = await _botService.GetGuildAsync(guildId);
                var member = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));

                var hasPermission = await CheckIfHasPermissionAsync(member, discordGuild, guild);

                guild.TwitchPlugin.TwitchChannelSubscriptions = twitchPluginDto.TwitchChannelSubscriptions;
                
                if (twitchPluginDto.IsEnabled != guild.TwitchPlugin.IsEnabled)
                {
                    guild.TwitchPlugin.IsEnabled = twitchPluginDto.IsEnabled;
                }

                if (!hasPermission)
                {
                    return false;
                }

                var initiator =
                    await _userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);

                // TODO: log
                
                await _pluginRepository.SaveAllAsync();
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        
        public async Task<bool> UpdateClashAPIPluginAsync(string guildId, ClashAPIPluginDto clashAPIPluginDto, User user)
        {
            try
            {
                var guild = await _guildService.GetByGuildIdAsync(guildId);
                var discordGuild = await _botService.GetGuildAsync(guildId);
                var member = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));

                var hasPermission = await CheckIfHasPermissionAsync(member, discordGuild, guild);

                if (clashAPIPluginDto.IsEnabled != guild.ClashAPIPlugin.IsEnabled)
                {
                    guild.ClashAPIPlugin.IsEnabled = clashAPIPluginDto.IsEnabled;
                }

                if (!hasPermission)
                {
                    return false;
                }

                var initiator =
                    await _userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);

                // TODO: log
                
                await _pluginRepository.SaveAllAsync();
                
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> TriggerCustomCommandsPluginStateAsync(string guildId, User user)
        {
            try
            {
                var guild = await _guildService.GetByGuildIdAsync(guildId);
                var discordGuild = await _botService.GetGuildAsync(guildId);
                var member = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));

                var hasPermission = await CheckIfHasPermissionAsync(member, discordGuild, guild);

                if (!hasPermission)
                {
                    return false;
                }

                var initiator =
                    await _userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                
                guild.CustomCommandPlugin.IsEnabled = !guild.CustomCommandPlugin.IsEnabled;

                var stateString = guild.CustomCommandPlugin.IsEnabled ? "ENABLED" : "DISABLED";
                await _logService.AddAsync(
                    $"{stateString}_CUSTOM_COMMANDS_PLUGIN",
                    ActionType.UPDATE,
                    initiator,
                    guildId
                );
                await _pluginRepository.SaveAllAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> AddCustomCommandAsync(string guildId, CustomCommandDto customCommandDto, User user)
        {
            try
            {
                var guild = await _guildService.GetByGuildIdAsync(guildId);

                var discordGuild = await _botService.GetGuildAsync(guildId);
                var member = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));

                var hasPermission = await CheckIfHasPermissionAsync(member, discordGuild, guild);

                if (!hasPermission)
                {
                    return false;
                }

                var initiator =
                    await _userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                guild.CustomCommandPlugin.Commands.Add(new Command
                {
                    Description = customCommandDto.Description,
                    Prefix = customCommandDto.Prefix,
                    CommandCall = customCommandDto.CommandCall,
                    IsEnabled = true,
                });

                await _logService.AddAsync(
                    $"ADDED_A_CUSTOM_COMMAND",
                    ActionType.UPDATE,
                    initiator,
                    guildId
                );
                await _pluginRepository.SaveAllAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> AddCustomAdvancedCommandsAsync(string guildId, IList<CustomAdvancedCommandDto> customAdvancedCommandDtos, User user)
        {
            try
            {
                var guild = await _guildService.GetByGuildIdAsync(guildId);

                var discordGuild = await _botService.GetGuildAsync(guildId);
                var member = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));

                var hasPermission = await CheckIfHasPermissionAsync(member, discordGuild, guild);

                if (!hasPermission)
                {
                    return false;
                }

                var initiator =
                    await _userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                foreach (var command in customAdvancedCommandDtos)
                {
                    guild.CustomCommandPlugin.AdvancedCommands.Add(new AdvancedCommand
                    {
                        Actions = command.Actions,
                        Description = command.Description,
                        Prefix = command.Prefix,
                        CommandCall = command.CommandCall,
                        IsEnabled = command.IsEnabled
                    });
                }

                await _logService.AddAsync($"ADDED_AN_ADVANCED_CUSTOM_COMMAND",
                    ActionType.UPDATE,
                    initiator,
                    guildId
                );
                await _pluginRepository.SaveAllAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> AddBlacklistedWordsAsync(string guildId, string[] words, User user)
        {
            if (words.IsNullOrEmpty())
            {
                return false;
            }

            var guild = await _guildService.GetByGuildIdAsync(guildId);

            var discordGuild = await _botService.GetGuildAsync(guildId);
            var member = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));

            var hasPermission = await CheckIfHasPermissionAsync(member, discordGuild, guild);

            if (!hasPermission)
            {
                return false;
            }

            var initiator =
                await _userService.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);

            foreach (var word in words)
            {
                guild.AutoModPlugin.BadWordsSettings.BadWords.Add(new BadWord { Guild = guild, Word = word });
                await _logService.AddAsync($"Added blacklisted word: {word}", ActionType.CREATE, initiator, guildId);
            }

            await _pluginRepository.SaveAllAsync();
            
            return true;
        }
    }
}