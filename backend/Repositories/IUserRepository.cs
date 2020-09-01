using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Data
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        // Task<User> GetByAccessTokenAsync(string id);
        Task<User> GetByUsernameAndDiscriminatorAsync(string username, string discriminator);
        Task DeleteByIdAsync(Guid id);
        Task<User> AddAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}