using backend.Data;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogService _logService;
        private readonly IPatchNoteService _patchNoteService;

        public UserService(IUserRepository userRepository, ILogService logService, IPatchNoteService patchNoteService)
        {
            _userRepository = userRepository;
            _logService = logService;
            _patchNoteService = patchNoteService;
        }
        public async Task<IList<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<IList<User>> GetByUsernameAsync(string userName)
        {
            var users = await _userRepository.GetAllAsync();
            return users.Where(u => u.UserName == userName).ToList();
        }

        /*
        public async Task<User> GetByAccessTokenAsync(string accessToken)
        {
            return await _userRepository.GetByAccessTokenAsync(accessToken);
        }
        */

        public async Task<User> GetByUsernameAndDiscriminatorAsync(string userName, string discriminator)
        {
            return await _userRepository.GetByUsernameAndDiscriminatorAsync(userName, discriminator);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        /*
        public async Task<string> GetAccessTokenAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user.AccessToken;
        }
        */

        public async Task<bool> SuspendUserAsync(Guid userId, DateTime expiresAt, User user)
        {
            try
            {
                var contestedUser = await _userRepository.GetByIdAsync(userId);
                var initiator =
                    await _userRepository.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);

                // Check if user is already suspended or banned
                if (contestedUser.LockoutEnd != null)
                {
                    // If so, do not do anything
                    return false;
                }

                contestedUser.LockoutEnd = expiresAt;

                await _logService.AddAsync(
                    $"Suspended user: {contestedUser.UserName}#{contestedUser.Discriminator}",
                    ActionType.UPDATE,
                    initiator,
                    null
                    );
                await _userRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> BanUserAsync(Guid userId, User user)
        {
            try
            {
                var contestedUser = await _userRepository.GetByIdAsync(userId);
                var initiator =
                    await _userRepository.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);

                // Check if user is already suspended or banned
                if (contestedUser.LockoutEnd != null)
                {
                    // If so, do not do anything
                    return false;
                }

                contestedUser.LockoutEnd = DateTimeOffset.Now.AddYears(100);

                await _logService.AddAsync(
                    $"Banned user: {contestedUser.UserName}#{contestedUser.Discriminator}",
                    ActionType.UPDATE,
                    initiator,
                    null
                    );
                await _userRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid userId, User user)
        {
            try
            {
                var contestedUser = await _userRepository.GetByIdAsync(userId);
                var initiator =
                    await _userRepository.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);
                await _userRepository.DeleteByIdAsync(user.Id);
                await _logService.AddAsync(
                    $"Deleted user: {contestedUser.UserName}#{contestedUser.Discriminator}",
                    ActionType.DELETE,
                    initiator,
                    null
                    );
                await _userRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> EnableUserAsync(Guid userId, User user)
        {
            try
            {
                var contestedUser = await _userRepository.GetByIdAsync(userId);
                var initiator =
                    await _userRepository.GetByUsernameAndDiscriminatorAsync(user.UserName, user.Discriminator);

                // Check if user is banned or suspended
                if (contestedUser.LockoutEnd == null)
                {
                    // If not, user is already enabled
                    return false;
                }

                contestedUser.LockoutEnd = null;

                await _logService.AddAsync(
                    $"Enabled user: {contestedUser.UserName}#{contestedUser.Discriminator}",
                    ActionType.UPDATE, initiator,
                    null
                    );
                await _userRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await _userRepository.DeleteByIdAsync(id);
        }

        public async Task<User> AddAsync(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task<IList<PatchNote>> GetUnseenPatchNotesAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            var patchNotes = await _patchNoteService.GetAllAsync();
            var relevantPatchNotes = patchNotes
                .Where(p => !user.SeenPatchNotes.Select(a => a.PatchNoteId).ToList().Contains(p.Id)).ToList();
            relevantPatchNotes = relevantPatchNotes.Where(p => p.CreatedAt > user.CreatedAt).ToList();

            return relevantPatchNotes;
        }

        public async Task<bool> MarkPatchNoteAsSeenAsync(Guid patchNoteId, Guid userId)
        {
            try
            {
                var user = await GetByIdAsync(userId);

                var seenPatchNote = new SeenPatchNote();
                seenPatchNote.PatchNoteId = patchNoteId;

                user.SeenPatchNotes.Add(seenPatchNote);

                await _userRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
    }
}