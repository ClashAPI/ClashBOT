using backend.DTOs;
using backend.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace backend.Services
{
    public class DiscordDataService : IDiscordDataService
    {
        private readonly HttpClient _client;
        private readonly IBotService _botService;
        private readonly DiscordAppDetails _discordAppDetails;

        public DiscordDataService(HttpClient client, IOptions<DiscordAppDetails> discordAppDetails,
            IBotService botService)
        {
            _client = client;
            _botService = botService;
            _discordAppDetails = discordAppDetails.Value;
        }
        public async Task<DiscordMemberDto> GetDiscordUserDetailsAsync(string accessToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync("https://discordapp.com/api/v6/users/@me");
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JObject.Parse(responseString);
            var responseObjectConverted = responseObject.ToObject<DiscordMemberDto>();

            return responseObjectConverted;
        }

        public async Task<List<GuildDto>> GetDiscordUserGuildsAsync(string accessToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync("https://discordapp.com/api/v6/users/@me/guilds");
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JArray.Parse(responseString);
            var responseObjectConverted = responseObject.ToObject<List<GuildDto>>();

            foreach (var guildDto in responseObjectConverted)
            {
                if (guildDto.Owner)
                {
                    var isMember = _botService.GetIfBotIsMemberOfGuild(guildDto.Id);
                    guildDto.IsAvailable = isMember;
                }
                else
                {
                    guildDto.IsAvailable = false;
                }

            }

            return responseObjectConverted;
        }

        /*
        public async Task<JObject> GetDiscordUserAccessTokenAsync(string code)
        {
            var data = new Dictionary<string, string>
            {
                {"client_id", _discordAppDetails.ClientId},
                {"client_secret", _discordAppDetails.ClientSecret},
                {"code", code},
                {"redirect_uri", "http://localhost:4200/login/callback"},
                {"scope", "identify guilds"},
                {"grant_type", "authorization_code"}
            };

            var response = await _client.PostAsync("https://discordapp.com/api/v6/oauth2/token", new FormUrlEncodedContent(data));
            var responseString = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseString);
        }
        */
    }
}