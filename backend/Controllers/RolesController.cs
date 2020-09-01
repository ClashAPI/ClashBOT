using AutoMapper;
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
    [ApiController]
    [Route("api/v1/{guildId}/roles")]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IBotService _botService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public RolesController(
            IBotService botService,
            IMapper mapper,
            UserManager<User> userManager
            )
        {
            _botService = botService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole([FromRoute] string guildId, [FromRoute] ulong id)
        {
            return Ok(await _botService.GetRoleAsync(guildId, id));
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles([FromRoute] string guildId)
        {
            var discordRoles = await _botService.GetRolesAsync(guildId);
            var roles = new List<DiscordRoleForListDto>();

            foreach (var discordRole in discordRoles)
            {
                var discordRoleForListDto = _mapper.Map<DiscordRoleForListDto>(discordRole);
                discordRoleForListDto.RoleId = discordRole.Id.ToString();
                roles.Add(discordRoleForListDto);
            }

            return Ok(roles);
        }

        [HttpGet("{roleId}/members")]
        public async Task<IActionResult> GetMembersInRole([FromRoute] string guildId, [FromRoute] ulong roleId)
        {
            return Ok(await _botService.GetMembersInRoleAsync(guildId, roleId));
        }

        [HttpPost("grant")]
        public async Task<IActionResult> GrantRoleAsync([FromRoute] string guildId, RoleActionDto roleActionDto)
        {
            await _botService.GrantRoleAsync(
                guildId, roleActionDto.UserId,
                roleActionDto.RoleId,
                await _userManager.GetUserAsync(User)
                );

            return Ok();
        }

        [HttpPost("revoke/multiple")]
        public async Task<IActionResult> RevokeRolesAsync([FromRoute] string guildId, RevokeRolesDto revokeRolesDto)
        {
            await _botService.RevokeRolesAsync(
                guildId, revokeRolesDto.UserId,
                revokeRolesDto.DiscordRoles,
                await _userManager.GetUserAsync(User)
                );

            return Ok();
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRoleAsync([FromRoute] string guildId, RoleActionDto roleActionDto)
        {
            await _botService.RevokeRoleAsync(
                guildId,
                roleActionDto.UserId,
                roleActionDto.RoleId,
                await _userManager.GetUserAsync(User),
                roleActionDto.Reason ?? "No reason specified."
                );

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string guildId, [FromRoute] ulong id)
        {
            var result = await _botService.DeleteRoleAsync(
                    guildId,
                    id,
                    await _userManager.GetUserAsync(User)
            );

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}