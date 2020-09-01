using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Repositories.Implementation
{
    public class GuildRepository : IGuildRepository
    {
        private readonly ApplicationDbContext _context;

        public GuildRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool RemoveTemporaryBan(TemporaryBan temporaryBan)
        {
            try
            {
                _context.Remove(temporaryBan);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public async Task<List<Guild>> GetAllAsync()
        {
            return await _context.Guilds.ToListAsync();
        }

        public async Task<Guild> GetByGuildIdAsync(string id)
        {
            return await _context.Guilds.FirstOrDefaultAsync(g => g.GuildId == id);
        }

        public async Task<Guild> AddAsync(Guild guild)
        {
            var dbGuild = await _context.Guilds.AddAsync(guild);
            return dbGuild.Entity;
        }
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}