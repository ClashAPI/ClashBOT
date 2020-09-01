using DSharpPlus;
using DSharpPlus.Entities;
using System.Collections.Generic;
using DiscordChannel = DSharpPlus.Entities.DiscordChannel;

namespace backend.DTOs
{
    public class DiscordChannelForListDto
    {
        public string ChannelId { get; set; }
        public ulong GuildId { get; set; }
        public ulong? ParentId { get; set; }
        public DiscordChannel Parent { get; set; }
        public string Name { get; set; }
        public ChannelType Type { get; set; }
        public int Position { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsCategory { get; set; }
        public DiscordGuild Guild { get; set; }
        public IReadOnlyList<DiscordOverwrite> PermissionOverwrites { get; set; }
        public string Topic { get; set; }
        public ulong LastMessageId { get; set; }
        public int Bitrate { get; set; }
        public int UserLimit { get; set; }
        public string Mention { get; set; }
        // public IEnumerable<DiscordChannel> Children { get; set; }
        public bool IsNSFW { get; set; }
    }
}