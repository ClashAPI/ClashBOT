using Pekka.ClashRoyaleApi.Client.Contracts;
using Pekka.ClashRoyaleApi.Client.Models.ClanModels;
using Pekka.ClashRoyaleApi.Client.Models.PlayerModels;
using Pekka.Core.Contracts;
using System.Threading.Tasks;

namespace backend.Services
{
    public class ClashRoyaleService : IClashRoyaleService
    {
        private readonly IRestApiClient _restApiClient;
        private readonly IPlayerClient _playerClient;
        private readonly IClanClient _clanClient;
        // private static readonly ServiceCollection Services = new ServiceCollection();
        // private static readonly ServiceProvider BuildServiceProvider = Services.BuildServiceProvider();

        public ClashRoyaleService(IRestApiClient restApiClient, IPlayerClient playerClient, IClanClient clanClient)
        {
            _restApiClient = restApiClient;
            _playerClient = playerClient;
            _clanClient = clanClient;
        }


        public async Task<Player> GetPlayerAsync(string playerTag)
        {
            return await _playerClient.GetPlayerAsync("#" + playerTag);
        }

        public async Task<Clan> GetClanAsync(string clanTag)
        {
            return await _clanClient.GetClanAsync("#" + clanTag);
        }

        public string GetNormalizedClanRole(string role)
        {
            switch (role)
            {
                case "member":
                    return "Member";
                case "elder":
                    return "Elder";
                case "coLeader":
                    return "Co-leader";
                case "leader":
                    return "Leader";
                default:
                    return "N/A";
            }
        }
    }
}