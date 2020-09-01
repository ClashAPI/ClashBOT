using DSharpPlus.Entities;
using System;

namespace backend.Models
{
    public class Kick
    {
        public DiscordMember DiscordMember { get; set; }
        public string Reason { get; set; }
        public DateTime KickedAt { get; set; }
    }
}