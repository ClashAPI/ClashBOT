using backend.Data;
using backend.Helpers;
using backend.Models;
using System;
using System.Threading.Tasks;

namespace backend.Repositories.Implementation
{
    public class PluginRepository : IPluginRepository
    {
        private readonly ApplicationDbContext _context;

        public PluginRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool RemoveModeratorRole(ModeratorRole moderatorRole)
        {
            try
            {
                _context.Remove(moderatorRole);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool RemoveAllowedRole(DiscordRole discordRole)
        {
            try
            {
                _context.Remove(discordRole);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}