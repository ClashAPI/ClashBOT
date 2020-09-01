using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/{guildId}")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IBotService _botService;
        private readonly UserManager<User> _userManager;

        public MessagesController(IBotService botService, UserManager<User> userManager)
        {
            _botService = botService;
            _userManager = userManager;
        }

        [HttpGet("channels")]
        public async Task<IActionResult> GetChannels([FromRoute] string guildId)
        {
            return Ok(await _botService.GetChannelsAsync(guildId));
        }



        [HttpGet("guild")]
        public async Task<IActionResult> GetGuild([FromRoute] string guildId)
        {
            return Ok(await _botService.GetGuildAsync(guildId));
        }
    }
}