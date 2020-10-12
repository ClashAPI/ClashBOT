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
        Task<bool> UpdateTwitchPluginAsync(string guildId, TwitchPluginDto twitchPluginDto, User user);
        Task<bool> UpdateClashAPIPluginAsync(string guildId, ClashAPIPluginDto clashAPIPluginDto, User user);

        Task<bool> UpdateScheduledMessagesPluginAsync(string guildId,
            ScheduledMessagesPluginDto scheduledMessagesPluginDto, User user);
    }
}