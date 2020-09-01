using AutoMapper;
using backend.DTOs;
using backend.Models;
using DSharpPlus.Entities;
using System.Collections.Generic;
using DiscordChannel = DSharpPlus.Entities.DiscordChannel;
using DiscordRole = DSharpPlus.Entities.DiscordRole;

namespace backend.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<DiscordMember, DiscordMemberForListDto>();
            CreateMap<DiscordRole, DiscordRoleForListDto>();
            CreateMap<DiscordChannel, DiscordChannelForListDto>();
            CreateMap<GuildListDto, List<Guild>>();
        }
    }
}