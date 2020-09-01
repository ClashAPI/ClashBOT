using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface ILogService
    {
        Task<LogEntry> GetByIdAsync(Guid id);
        Task<List<LogEntry>> GetAllAsync();
        Task<List<LogEntry>> GetAllByGuildIdAsync(string guildId);
        Task<bool> AddAsync(string actionName, ActionType actionType, User initiator, string? guildId);
        Task<bool> DeleteByIdAsync(Guid id);
        Task<List<LogEntry>> DeleteAllByGuildIdAsync(string guildId, User initiator);
    }
}