using Pekka.ClashRoyaleApi.Client.Models.ClanModels;
using Pekka.ClashRoyaleApi.Client.Models.PlayerModels;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IClashRoyaleService
    {
        Task<Player> GetPlayerAsync(string playerTag);
        Task<Clan> GetClanAsync(string clanTag);
        string GetNormalizedClanRole(string role);
    }
}