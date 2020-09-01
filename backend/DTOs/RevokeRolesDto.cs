using DSharpPlus.Entities;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class RevokeRolesDto
    {
        public ulong UserId { get; set; }
        public IList<DiscordRole> DiscordRoles { get; set; }
    }
}