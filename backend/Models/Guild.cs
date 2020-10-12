using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Guild
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string GuildId { get; set; }
        public virtual List<User> Managers { get; set; }
        public virtual ClashAPIPlugin ClashAPIPlugin { get; set; }
        public virtual AutoModPlugin AutoModPlugin { get; set; }
        public virtual CustomCommandPlugin CustomCommandPlugin { get; set; }
        public virtual NotificationsPlugin NotificationsPlugin { get; set; }
        public virtual List<TemporaryBan> TemporaryBans { get; set; }
        public virtual List<Infraction> Infractions { get; set; }
        public virtual ScheduledMessagesPlugin ScheduledMessagesPlugin { get; set; }
        public virtual TwitchPlugin TwitchPlugin { get; set; }

        public Guild()
        {
            AutoModPlugin = new AutoModPlugin
            {
                IsEnabled = false,
                ZalgoSettings = new ZalgoSettings(),
                BadWordsSettings = new BadWordsSettings(),
                ExcessiveCapsSettings = new ExcessiveCapsSettings(),
                ExcessiveEmojisSettings = new ExcessiveEmojisSettings(),
                ExcessiveMentionsSettings = new ExcessiveMentionsSettings(),
                ExcessiveSpoilersSettings = new ExcessiveSpoilersSettings(),
                ExternalLinksSettings = new ExternalLinksSettings(),
                RepeatedTextSettings = new RepeatedTextSettings(),
                ServerInvitesSettings = new ServerInvitesSettings(),
                ModeratorCommands = new List<ModeratorCommand>
                {
                new ModeratorCommand
                {
                    CommandCall = "ban",
                    Prefix = '!',
                    Description = "BAN_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "tempban",
                    Prefix = '!',
                    Description = "TEMPBAN_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "clear",
                    Prefix = '!',
                    Description = "CLEAR_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "clear-all-infractions",
                    Prefix = '!',
                    Description = "CLEAR_ALL_INFRACTIONS_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "infractions",
                    Prefix = '!',
                    Description = "INFRACTIONS_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "kick",
                    Prefix = '!',
                    Description = "KICK_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "mute",
                    Prefix = '!',
                    Description = "MUTE_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "tempmute",
                    Prefix = '!',
                    Description = "TEMPMUTE_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "role-info",
                    Prefix = '!',
                    Description = "ROLE_INFO_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "server-info",
                    Prefix = '!',
                    Description = "GUILD_INFO_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "slowmode",
                    Prefix = '!',
                    Description = "SLOW_MODE_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "unban",
                    Prefix = '!',
                    Description = "UNBAN_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "unmute",
                    Prefix = '!',
                    Description = "UNMUTE_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "user-info",
                    Prefix = '!',
                    Description = "USER_INFO_COMMAND_DESCRIPTION",
                    IsEnabled = true
                },
                new ModeratorCommand
                {
                    CommandCall = "warn",
                    Prefix = '!',
                    Description = "WARN_COMMAND_DESCRIPTION",
                    IsEnabled = true
                }}
            };

            ClashAPIPlugin = new ClashAPIPlugin
            {
                IsEnabled = false
            };

            CustomCommandPlugin = new CustomCommandPlugin
            {
                IsEnabled = false
            };

            NotificationsPlugin = new NotificationsPlugin
            {
                IsEnabled = false
            };
            ScheduledMessagesPlugin = new ScheduledMessagesPlugin
            {
                IsEnabled = false,
                ScheduledMessages = new List<ScheduledMessage>()
            };
            TwitchPlugin = new TwitchPlugin
            {
                IsEnabled = false
            };
        }
    }
}