using DSharpPlus.Entities;
using System;

namespace backend.Models
{
    public class Ban
    {
        public DiscordMember DiscordMember { get; set; }
        public string Reason { get; set; }
        public DateTime BannedAt { get; set; }
    }
}