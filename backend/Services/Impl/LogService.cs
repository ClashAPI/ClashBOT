using backend.Data;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        public async Task<LogEntry> GetByIdAsync(Guid id)
        {
            return await _logRepository.GetByIdAsync(id);
        }

        public async Task<List<LogEntry>> GetAllAsync()
        {
            return await _logRepository.GetAllAsync();
        }

        public async Task<List<LogEntry>> GetAllByGuildIdAsync(string guildId)
        {
            return await _logRepository.GetAllByGuildIdAsync(guildId);
        }

        public async Task<bool> AddAsync(string actionName, ActionType actionType, User initiator, string? guildId)
        {
            try
            {
                var logEntry = new LogEntry
                {
                    ActionName = actionName,
                    ActionType = actionType,
                    Initiator = initiator,
                    GuildId = guildId
                };

                await _logRepository.AddAsync(logEntry);
                await _logRepository.SaveAllAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            try
            {
                await _logRepository.DeleteByIdAsync(id);
                await _logRepository.SaveAllAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<LogEntry>> DeleteAllByGuildIdAsync(string guildId, User user)
        {
            try
            {
                var initiator = await _logRepository.GetUserByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                var logs = await _logRepository.GetAllByGuildIdAsync(guildId);
                _logRepository.DeleteAll(logs);

                await _logRepository.AddAsync(new LogEntry
                {
                    ActionName = $"DELETED_ALL_LOG_ENTRIES",
                    ActionType = ActionType.UPDATE,
                    Initiator = initiator,
                    GuildId = guildId
                });
                await _logRepository.SaveAllAsync();

                return await _logRepository.GetAllByGuildIdAsync(guildId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
    }
}