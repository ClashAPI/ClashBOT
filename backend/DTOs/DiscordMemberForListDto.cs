using DSharpPlus.Entities;
using System;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class DiscordMemberForListDto
    {
        public string MemberId { get; set; }
        public DiscordColor Color { get; set; }
        public string Discriminator { get; set; }
        public string Email { get; set; }
        public DiscordGuild Guild { get; set; }
        public string Nickname { get; set; }
        public IEnumerable<DiscordRole> Roles { get; set; }
        public string Username { get; set; }
        public bool? Verified { get; set; }
        public string AvatarHash { get; set; }
        public string DisplayName { get; set; }
        public bool IsBot { get; set; }
        public bool IsDeafened { get; set; }
        public bool IsMuted { get; set; }
        public bool IsOwner { get; set; }
        public DateTimeOffset JoinedAt { get; set; }
        public bool? MfaEnabled { get; set; }
        public DiscordVoiceState VoiceState { get; set; }
        public ulong Id { get; set; }
        public string Mention { get; set; }
        public DiscordPresence Presence { get; set; }
        public string AvatarUrl { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }
        public bool IsCurrent { get; set; }
        public string DefaultAvatarUrl { get; set; }
    }
}