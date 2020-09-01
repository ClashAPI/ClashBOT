using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Data
{
    public interface ILogRepository
    {
        Task<LogEntry> GetByIdAsync(Guid id);
        Task<List<LogEntry>> GetAllAsync();

        Task<List<LogEntry>> GetAllByGuildIdAsync(string id);
        Task AddAsync(LogEntry logEntry);
        Task DeleteByIdAsync(Guid id);
        void DeleteAll(IList<LogEntry> logEntries);
        Task<bool> SaveAllAsync();
        Task<User> GetUserByUsernameAndDiscriminatorAsync(string username, string discriminator);
    }
}