using backend.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IDiscordDataService
    {
        Task<DiscordMemberDto> GetDiscordUserDetailsAsync(string accessToken);
        Task<List<GuildDto>> GetDiscordUserGuildsAsync(string accessToken);
        // Task<JObject> GetDiscordUserAccessTokenAsync(string code);
    }
}