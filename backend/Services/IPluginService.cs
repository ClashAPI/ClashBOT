using backend.DTOs;
using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IPluginService
    {
        Task<bool> UpdateCustomCommandsPluginAsync(string guildId, CustomCommandsPluginDto customCommandPluginDto,
            User initiator);

        Task<bool> TriggerCustomCommandsPluginStateAsync(string guildId, User initiator);
        Task<bool> AddCustomCommandAsync(string guildId, CustomCommandDto customCommandDto, User initiator);
        Task<bool> UpdateAutomodPluginAsync(string guildId, AutoModPluginDto autoModPluginDto, User initiator);
        Task<bool> AddBlacklistedWordsAsync(string guildId, string[] words, User user);
        Task<bool> AddCustomAdvancedCommandsAsync(string guildId, IList<CustomAdvancedCommandDto> customAdvancedCommandDtos, User user);
    }
}