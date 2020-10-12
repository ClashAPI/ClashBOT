using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/v1/{guildId}/plugins")]
    [Authorize]
    public class PluginsController : ControllerBase
    {
        private readonly IPluginService _pluginService;
        private readonly IGuildService _guildService;
        private readonly UserManager<User> _userManager;

        public PluginsController(IPluginService pluginService, IGuildService guildService, UserManager<User> userManager)
        {
            _pluginService = pluginService;
            _guildService = guildService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetGuildPlugins([FromRoute] string guildId)
        {
            var guild = await _guildService.GetByGuildIdAsync(guildId);

            return Ok(new { guild.AutoModPlugin, guild.CustomCommandPlugin, guild.ClashAPIPlugin, guild.TwitchPlugin, guild.ScheduledMessagesPlugin });
        }

        [HttpGet("mod")]
        public async Task<IActionResult> GetGuildModPlugin([FromRoute] string guildId)
        {
            var guild = await _guildService.GetByGuildIdAsync(guildId);

            return Ok(guild.AutoModPlugin);
        }

        [HttpPost("mod/words")]
        public async Task<IActionResult> AddBlacklistedWords([FromRoute] string guildId, [FromBody] string[] words)
        {
            await _pluginService
                .AddBlacklistedWordsAsync(
                    guildId,
                    words,
                    await _userManager.GetUserAsync(User)
                    );

            return Ok();
        }

        [HttpGet("scheduledmessages")]
        public async Task<IActionResult> GetScheduledMessagesPlugin([FromRoute] string guildId)
        {
            var guild = await _guildService.GetByGuildIdAsync(guildId);

            return Ok(guild.ScheduledMessagesPlugin);
        }
        
        [HttpPatch("scheduledmessages")]
        public async Task<IActionResult> UpdateScheduledMessagesPlugin([FromRoute] string guildId,
            [FromBody] ScheduledMessagesPluginDto scheduledMessagesPluginDto)
        {
            var result = await _pluginService.UpdateScheduledMessagesPluginAsync(guildId, scheduledMessagesPluginDto,
                await _userManager.GetUserAsync(User));

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("twitch")]
        public async Task<IActionResult> GetGuildTwitchPlugin([FromRoute] string guildId)
        {
            var guild = await _guildService.GetByGuildIdAsync(guildId);

            return Ok(guild.TwitchPlugin);
        }

        [HttpPatch("twitch")]
        public async Task<IActionResult> UpdateTwitchPlugin([FromRoute] string guildId,
            [FromBody] TwitchPluginDto twitchPluginDto)
        {
            var result =
                await _pluginService.UpdateTwitchPluginAsync(guildId, twitchPluginDto,
                    await _userManager.GetUserAsync(User));

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("clashapi")]
        public async Task<IActionResult> GetClashAPIPlugin([FromRoute] string guildId)
        {
            var guild = await _guildService.GetByGuildIdAsync(guildId);

            return Ok(guild.ClashAPIPlugin);
        }

        [HttpPatch("clashapi")]
        public async Task<IActionResult> UpdateClashAPIPlugin([FromRoute] string guildId,
            [FromBody] ClashAPIPluginDto clashAPIPluginDto)
        {
            var result = await _pluginService.UpdateClashAPIPluginAsync(guildId, clashAPIPluginDto,
                await _userManager.GetUserAsync(User));

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("customcommand")]
        public async Task<IActionResult> GetGuildCustomCommandsPlugin([FromRoute] string guildId)
        {
            var guild = await _guildService.GetByGuildIdAsync(guildId);

            return Ok(guild.CustomCommandPlugin);
        }

        [HttpPatch("customcommand")]
        public async Task<IActionResult> UpdateCustomCommandsPlugin([FromRoute] string guildId,
            [FromBody] CustomCommandsPluginDto customCommandsPluginDto)
        {
            var result = await _pluginService.UpdateCustomCommandsPluginAsync(guildId, customCommandsPluginDto,
                await _userManager.GetUserAsync(User));

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        // Reverses the "isActive" property of the custom commands plugin
        [HttpPatch("customcommand/isenabled")]
        public async Task<IActionResult> TriggerCustomCommandsPluginState([FromRoute] string guildId)
        {
            var result = await _pluginService.TriggerCustomCommandsPluginStateAsync(
                guildId,
                await _userManager.GetUserAsync(User));

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("customcommand/commands")]
        public async Task<IActionResult> AddCustomCommand([FromRoute] string guildId, CustomCommandDto customCommandDto)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _pluginService.AddCustomCommandAsync(guildId, customCommandDto, user);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("customcommand/advancedcommands")]
        public async Task<IActionResult> AddCustomAdvancedCommand([FromRoute] string guildId,
            [FromBody] IList<CustomAdvancedCommandDto> advancedCommands)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _pluginService.AddCustomAdvancedCommandsAsync(guildId, advancedCommands, user);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPatch("mod")]
        public async Task<IActionResult> UpdateAutoModPlugin([FromRoute] string guildId, [FromBody] AutoModPluginDto autoModPluginDto)
        {
            var result = await _pluginService
                .UpdateAutomodPluginAsync(
                    guildId,
                    autoModPluginDto,
                    await _userManager.GetUserAsync(User)
                    );

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}