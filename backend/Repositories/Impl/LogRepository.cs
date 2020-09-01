using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _context;

        public LogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<LogEntry> GetByIdAsync(Guid id)
        {
            return await _context.LogEntries.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<List<LogEntry>> GetAllAsync()
        {
            return await _context.LogEntries.ToListAsync();
        }

        public async Task<List<LogEntry>> GetAllByGuildIdAsync(string id)
        {
            return await _context.LogEntries.Where(e => e.GuildId == id).ToListAsync();
        }

        public async Task AddAsync(LogEntry logEntry)
        {
            await _context.LogEntries.AddAsync(logEntry);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var entry = await _context.LogEntries.FirstAsync(e => e.Id == id);
            _context.LogEntries.Remove(entry);
        }

        public void DeleteAll(IList<LogEntry> logEntries)
        {
            _context.LogEntries.RemoveRange(logEntries);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserByUsernameAndDiscriminatorAsync(string username, string discriminator)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Discriminator == discriminator);
        }
    }
}