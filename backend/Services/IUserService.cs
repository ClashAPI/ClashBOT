using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public interface IUserService
    {
        Task<IList<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id);
        Task<IList<User>> GetByUsernameAsync(string userName);
        // Task<User> GetByAccessTokenAsync(string accessToken);
        Task<User> GetByUsernameAndDiscriminatorAsync(string userName, string discriminator);
        // Task<string> GetAccessTokenAsync(Guid userId);
        Task<bool> BanUserAsync(Guid userId, User initiator);
        Task<bool> SuspendUserAsync(Guid userId, DateTime expiresAt, User initiator);
        Task<bool> DeleteUserAsync(Guid userId, User initiator);
        Task<bool> EnableUserAsync(Guid userId, User initiator);
        Task DeleteByIdAsync(Guid userId);
        Task<User> AddAsync(User user);
        Task<IList<PatchNote>> GetUnseenPatchNotesAsync(Guid userId);
        Task<bool> MarkPatchNoteAsSeenAsync(Guid patchNoteId, Guid userId);
        Task<bool> UpdatePreferencesAsync(AppPreferences appPreferences, Guid userId);
    }
}