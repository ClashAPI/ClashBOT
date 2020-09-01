using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        /*
        public async Task<User> GetByAccessTokenAsync(string accessToken)
        {
            return await _context.Users.FirstAsync(u => u.AccessToken == accessToken);
        }
        */

        public async Task<User> GetByUsernameAndDiscriminatorAsync(string username, string discriminator)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Discriminator == discriminator);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var user = await GetByIdAsync(id);
            _context.Users.Remove(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> AddAsync(User user)
        {
            var dbUser = await _context.Users.AddAsync(user);
            return dbUser.Entity;
        }
    }
}