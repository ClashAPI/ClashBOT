using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using backend.Models;
using backend.Services;
using DSharpPlus.Entities;
using Microsoft.AspNetCore.Identity;

namespace backend.Helpers
{
    public static class Utils
    {
        public static bool AuthorizeGuildRequest(DiscordGuild discordGuild, Guild dbGuild, User user)
        {
            if (discordGuild.Owner.Id.ToString() == user.UserId)
            {
                return true;
            }

            if (dbGuild.Managers.Select(m => m.UserId).ToList().Contains(user.UserId))
            {
                return true;
            }

            return false;
        }
    }
}
