using backend.Data;
using backend.DTOs;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.Entities;

namespace backend.Services
{
    public class GuildService : IGuildService
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IUserService _userService;
        private readonly IBotService _botService;

        public GuildService(IGuildRepository guildRepository, IUserService userService, IBotService botService)
        {
            _guildRepository = guildRepository;
            _userService = userService;
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

        public async Task<Guild> GetByGuildIdAsync(string guildId)
        {
            return await _guildRepository.GetByGuildIdAsync(guildId);
        }

        public async Task<List<Guild>> GetAllAsync()
        {
            return await _guildRepository.GetAllAsync();
        }

        public async Task<List<Guild>> GetManagedGuildsAsync(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);
            var guilds = await _guildRepository.GetAllAsync();

            return guilds.Where(g => g.Managers.Contains(user)).ToList();
        }

        public async Task<List<TemporaryBan>> GetTemporaryBansAsync(string guildId, User user)
        {
            var guild = await _botService.GetGuildAsync(guildId);
            var dbGuild = await _guildRepository.GetByGuildIdAsync(guildId);
            var member = await guild.GetMemberAsync(ulong.Parse(user.UserId));
            var hasPermission = await CheckIfHasPermissionAsync(member, guild, dbGuild);

            if (!hasPermission)
            {
                return null;
            }

            return dbGuild.TemporaryBans;
        }

        public async Task<bool> AddTemporaryBansAsync(string guildId, List<TemporaryBanDto> temporaryBanDto, User user)
        {
            try
            {
                var guild = await _guildRepository.GetByGuildIdAsync(guildId);
                var discordGuild = await _botService.GetGuildAsync(guildId);

                var discordMember = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));
                var hasPermission = await CheckIfHasPermissionAsync(discordMember, discordGuild, guild);

                if (!hasPermission)
                {
                    return false;
                }

                foreach (var tempBan in temporaryBanDto)
                {
                    var member = discordGuild.Members.FirstOrDefault(m =>
                        m.Username == tempBan.Username && m.Discriminator == tempBan.Discriminator);
                    await discordGuild.RemoveMemberAsync(member);
                    guild.TemporaryBans.Add(new TemporaryBan
                    {
                        MemberId = tempBan.MemberId,
                        ExpiresAt = tempBan.ExpiresAt
                    });
                }

                await _guildRepository.SaveAllAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> RemoveTemporaryBansAsync(string guildId, List<TemporaryBanDto> temporaryBanDto, User user)
        {
            try
            {
                var guild = await _guildRepository.GetByGuildIdAsync(guildId);
                var discordGuild = await _botService.GetGuildAsync(guildId);

                var discordMember = await discordGuild.GetMemberAsync(ulong.Parse(user.UserId));
                var hasPermission = await CheckIfHasPermissionAsync(discordMember, discordGuild, guild);

                if (!hasPermission)
                {
                    return false;
                }

                foreach (var tempBan in temporaryBanDto)
                {
                    var ban = guild.TemporaryBans.First(b => b.MemberId == tempBan.MemberId);
                    _guildRepository.RemoveTemporaryBan(ban);
                }

                await _guildRepository.SaveAllAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<Guild> AddAsync(Guild guild)
        {
            return await _guildRepository.AddAsync(guild);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _guildRepository.SaveAllAsync();
        }
    }
}