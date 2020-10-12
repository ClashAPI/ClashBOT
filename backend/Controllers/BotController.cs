using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend.Helpers;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/bot")]
    [Authorize]
    [TypeFilter(typeof(SuperuserFilter))]
    public class BotController : ControllerBase
    {
        private readonly IBotService _botService;
        private readonly ITwitchService _twitchService;

        public BotController(IBotService botService, ITwitchService twitchService)
        {
            _botService = botService;
            _twitchService = twitchService;
        }

        [HttpPost("connect")]
        public async Task<IActionResult> Connect()
        {
            var result = await _botService.ConnectAsync();

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("disconnect")]
        public async Task<IActionResult> Disconnect()
        {
            await _botService.DisconnectAsync();
            return Ok();
        }

        [HttpPost("reconnect")]
        public async Task<IActionResult> Reconnect()
        {
            await _botService.ReconnectAsync();
            return Ok();
        }

        [HttpPost("{guildId}/status")]
        public IActionResult GetIfBotIsMemberOfGuild([FromRoute] string guildId)
        {
            return Ok(new { joined = _botService.GetIfBotIsMemberOfGuild(guildId) });
        }

        [HttpGet("ping")]
        public IActionResult GetBotPingAsync()
        {
            return Ok(_botService.GetPing());
        }

        [HttpGet("is-online")]
        public IActionResult GetIfBotOnline()
        {
            return Ok(_botService.GetIfBotOnline());
        }

        [HttpGet("twitch")]
        public IActionResult InjectTwitchService()
        {
            _twitchService.Init();
            
            return Ok();
        }
    }
}