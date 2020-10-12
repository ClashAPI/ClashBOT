using System;
using System.Threading.Tasks;
using TwitchLib.Api.Helix.Models.Users;

namespace backend.Services
{
    public interface ITwitchService
    {
        void Init();
        Task<User> GetUserByIdAsync(string id);
    }
}