using backend.DTOs;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IGuildService
    {
        Task<Guild> GetByGuildIdAsync(string guildId);
        Task<List<Guild>> GetAllAsync();
        Task<List<Guild>> GetManagedGuildsAsync(Guid userId);
        Task<List<TemporaryBan>> GetTemporaryBansAsync(string guildId, User user);
        Task<bool> AddTemporaryBansAsync(string guildId, List<TemporaryBanDto> temporaryBanDto, User user);
        Task<bool> RemoveTemporaryBansAsync(string guildId, List<TemporaryBanDto> temporaryBanDto, User user);
        Task<Guild> AddAsync(Guild guild);
        Task<bool> SaveAllAsync();
    }
}