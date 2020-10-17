using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using backend.Helpers;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGuildService _guildService;
        private readonly IDiscordDataService _discordDataService;
        private readonly UserManager<User> _userManager;

        public UsersController(
            IUserService userService,
            IGuildService guildService,
            IDiscordDataService discordDataService, UserManager<User> userManager
            )
        {
            _userService = userService;
            _guildService = guildService;
            _discordDataService = discordDataService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetAllAsync());
        }

        [HttpPost("{id}/guilds")]
        public async Task<IActionResult> GetUserGuilds([FromRoute] Guid id, [FromBody] TokenDto tokenDto)
        {
            return Ok(await _discordDataService.GetDiscordUserGuildsAsync(tokenDto.Token));
        }

        [HttpGet("{id}/guilds/{guildId}")]
        public async Task<IActionResult> GetUserGuild([FromRoute] Guid id, [FromRoute] string guildId)
        {
            return Ok(await _guildService.GetByGuildIdAsync(guildId));
        }

        [HttpPost("{id}/suspend")]
        [TypeFilter(typeof(SuperuserFilter))]
        public async Task<IActionResult> SuspendUserAsync([FromRoute] Guid id, SuspendUserDto suspendUserDto)
        {
            var result = await _userService.SuspendUserAsync(id, suspendUserDto.ExpiresAt, await _userManager.GetUserAsync(User));

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("{id}/ban")]
        [TypeFilter(typeof(SuperuserFilter))]
        public async Task<IActionResult> BanUserAsync([FromRoute] Guid id)
        {
            var result = await _userService.BanUserAsync(id, await _userManager.GetUserAsync(User));

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(SuperuserFilter))]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] Guid id)
        {
            var result = await _userService.DeleteUserAsync(id, await _userManager.GetUserAsync(User));

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("{id}/enable")]
        [TypeFilter(typeof(SuperuserFilter))]
        public async Task<IActionResult> EnableUserAsync([FromRoute] Guid id)
        {
            var result = await _userService.EnableUserAsync(id, await _userManager.GetUserAsync(User));

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet("patch-notes")]
        public async Task<IActionResult> GetUnseenPatchNotes()
        {
            var user = await _userManager.GetUserAsync(User);
            var patchNotes = await _userService.GetUnseenPatchNotesAsync(user.Id);

            return Ok(patchNotes);
        }

        [HttpPost("patch-notes/{patchNoteId}")]
        public async Task<IActionResult> MarkPatchNoteAsSeen([FromRoute] Guid patchNoteId)
        {
            var user = await _userManager.GetUserAsync(User);
            await _userService.MarkPatchNoteAsSeenAsync(patchNoteId, user.Id);

            return Ok();
        }

        [HttpGet("preferences")]
        public async Task<IActionResult> GetPreferences()
        {
            var user = await _userManager.GetUserAsync(User);

            return Ok(new { user.AppPreferences.Language, user.AppPreferences.Theme });
        }

        [HttpPatch("preferences")]
        public async Task<IActionResult> UpdatePreferences([FromBody] AppPreferences appPreferences)
        {
            var user = await _userManager.GetUserAsync(User);
            await _userService.UpdatePreferencesAsync(appPreferences, user.Id);

            return Ok();
        }
    }
}