using backend.DTOs;
using backend.Models;
using DSharpPlus.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordChannel = DSharpPlus.Entities.DiscordChannel;
using DiscordRole = DSharpPlus.Entities.DiscordRole;

namespace backend.Services
{
    public interface IBotService
    {
        int? GetPing();
        bool GetIfBotOnline();
        Task<IReadOnlyList<DiscordChannel>> GetChannelsAsync(string guildId);
        Task<bool> EditGuildAsync(GuildDto guildDto, User user);
        Task<DiscordGuild> GetGuildAsync(string guildId);
        Task<DiscordMember> GetMemberAsync(string guildId, ulong userId);
        Task<List<DiscordRole>> GetMemberRolesAsync(string guildId, ulong userId);
        Task<IReadOnlyList<DiscordMember>> GetMembersAsync(string guildId);
        Task<IReadOnlyList<DiscordMember>> GetMembersInRoleAsync(string guildId, ulong roleId);
        Task<DiscordRole> GetRoleAsync(string guildId, ulong roleId);
        Task<IReadOnlyList<DiscordRole>> GetRolesAsync(string guildId);
        Task<bool> GrantRoleAsync(string guildId, ulong userId, ulong roleId, User user);
        Task<bool> RevokeRoleAsync(string guildId, ulong userId, ulong roleId, User user, string reason);
        Task<bool> RevokeRolesAsync(string guildId, ulong userId, IList<DiscordRole> discordRoles, User initiator);
        Task<bool> RemoveMemberAsync(User user, string guildId, ulong userId, string reason,
            bool notifyUser, bool notifyAnonymously, bool includeReason);
        Task<bool> BanMemberAsync(User user, string guildId, ulong userId, string reason, bool notifyUser,
            bool notifyAnonymously, bool includeReason);
        Task<bool> DeleteRoleAsync(string guildId, ulong id, User initiator);
        Task<bool> ConnectAsync();
        Task<bool> DisconnectAsync();
        Task<bool> ReconnectAsync();
        Task<bool> HandleEditMemberAsync(string guildId, EditMemberDto editMemberDto, User initiator);
        // Task<bool> SetMemberDisplayNameAsync(string guildId, ulong userId, string nickname, string reason = null);
        // Task<bool> SetMemberDeafAsync(string guildId, ulong userId, bool isDeafened, string reason = null);
        // Task<bool> SetMemberMuteAsync(string guildId, ulong userId, bool isMuted, string reason = null);
        string GetClientUserId();
        bool GetIfBotIsMemberOfGuild(string guildId);

        Task<IReadOnlyList<DiscordVoiceRegion>> GetRegionsAsync();
        Task<IReadOnlyList<DiscordBan>> GetBansAsync(string guildId, User user);
        Task<bool> UnbanMembersAsync(string guildId, UnbanMembersDto unbanMembersDto, User user);
        Task<bool> SendTwitchNotification(string streamerId, TwitchNotificationType twitchNotificationType);
    }
}
