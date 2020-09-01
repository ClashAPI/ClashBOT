using DSharpPlus;
using DSharpPlus.Entities;
using System;

namespace backend.DTOs
{
    public class DiscordRoleForListDto
    {
        public string RoleId { get; set; }
        public DiscordColor Color { get; set; }
        public string Mention { get; set; }
        public string Name { get; set; }
        public Permissions Permissions { get; set; }
        public int Position { get; set; }
        public bool IsHoisted { get; set; }
        public bool IsManaged { get; set; }
        public bool IsMentionable { get; set; }
        public ulong Id { get; set; }
        public DateTimeOffset CreationTimestamp { get; set; }
    }
}