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
    [Route("api/v1/{guildId}/members")]
    [Authorize]
    public class MembersController : ControllerBase
    {
        private readonly IBotService _botService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public MembersController(IBotService botService, IMapper mapper,
            UserManager<User> userManager)
        {
            _botService = botService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember([FromRoute] string guildId, [FromRoute] ulong id)
        {
            return Ok(await _botService.GetMemberAsync(guildId, id));
        }

        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetMemberRoles([FromRoute] string guildId, [FromRoute] ulong id)
        {
            return Ok(await _botService.GetMemberRolesAsync(guildId, id));
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers([FromRoute] string guildId)
        {
            var discordMembers = await _botService.GetMembersAsync(guildId);
            var members = new List<DiscordMemberForListDto>();

            foreach (var discordMember in discordMembers)
            {
                var discordMemberForListDto = _mapper.Map<DiscordMemberForListDto>(discordMember);
                discordMemberForListDto.MemberId = discordMember.Id.ToString();
                members.Add(discordMemberForListDto);
            }

            return Ok(members);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveMemberAsync([FromRoute] string guildId, RemoveMemberDto removeMemberDto)
        {
            var user = await _userManager.GetUserAsync(User);

            await _botService.RemoveMemberAsync(
                user,
                guildId,
                ulong.Parse(removeMemberDto.UserId),
                removeMemberDto.Reason, removeMemberDto.NotifyUser,
                removeMemberDto.NotifyAnonymously,
                removeMemberDto.IncludeReason
                );

            return Ok();
        }

        [HttpPost("ban")]
        public async Task<IActionResult> BanMemberAsync([FromRoute] string guildId, RemoveMemberDto removeMemberDto)
        {
            var user = await _userManager.GetUserAsync(User);

            await _botService.BanMemberAsync(
                user,
                guildId,
                ulong.Parse(removeMemberDto.UserId),
                removeMemberDto.Reason,
                removeMemberDto.NotifyUser,
                removeMemberDto.NotifyAnonymously,
                removeMemberDto.IncludeReason
                );

            return Ok();
        }

        [HttpPost("unban")]
        public async Task<IActionResult> UnbanMembersAsync([FromRoute] string guildId, UnbanMembersDto unbanMembersDto)
        {
            var user = await _userManager.GetUserAsync(User);

            await _botService.UnbanMembersAsync(guildId, unbanMembersDto, user);

            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> EditMemberAsync(
            [FromRoute] string guildId,
            EditMemberDto editMemberDto,
            [FromRoute] ulong id
            )
        {
            if (editMemberDto.MemberId == null)
            {
                editMemberDto.MemberId = id.ToString();
            }

            await _botService.HandleEditMemberAsync(
                guildId, editMemberDto,
                await _userManager.GetUserAsync(User)
                );

            return Ok();
        }
    }
}