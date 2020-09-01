using backend.Helpers;
using backend.Models;
using System.Threading.Tasks;

namespace backend.Data
{
    public interface IPluginRepository
    {
        Task<bool> SaveAllAsync();
        bool RemoveModeratorRole(ModeratorRole moderatorRole);
        bool RemoveAllowedRole(DiscordRole discordRole);
    }
}