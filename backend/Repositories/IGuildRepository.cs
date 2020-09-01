using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Data
{
    public interface IGuildRepository
    {
        bool RemoveTemporaryBan(TemporaryBan temporaryBan);
        Task<Guild> GetByGuildIdAsync(string id);
        Task<List<Guild>> GetAllAsync();
        Task<Guild> AddAsync(Guild guild);
        Task<bool> SaveAllAsync();
    }
}