using AutoMapper;
using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Helpers;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/guilds")]
    [Authorize]
    public class GuildsController : ControllerBase
    {
        private readonly IBotService _botService;
        private readonly UserManager<User> _userManager;
        private readonly IGuildService _guildService;
        private readonly IMapper _mapper;

        public GuildsController(
            IBotService botService,
            UserManager<User> userManager,
            IGuildService guildService,
            IMapper mapper
            )
        {
            _botService = botService;
            _userManager = userManager;
            _guildService = guildService;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGuild([FromRoute] string id)
        {
            var guild = await _botService.GetGuildAsync(id);

            return Ok(guild);
        }

        [HttpGet("{id}/channels")]
        public async Task<IActionResult> GetChannels([FromRoute] string id)
        {
            var discordRoles = await _botService.GetChannelsAsync(id);
            var channels = new List<DiscordChannelForListDto>();

            foreach (var discordChannel in discordRoles)
            {
                var discordRoleForListDto = _mapper.Map<DiscordChannelForListDto>(discordChannel);
                discordRoleForListDto.ChannelId = discordChannel.Id.ToString();
                channels.Add(discordRoleForListDto);
            }

            return Ok(channels);
        }

        [HttpGet("managed")]
        public async Task<IActionResult> GetManagedGuilds()
        {
            var userId = _userManager.GetUserId(User);
            return Ok(await _guildService.GetManagedGuildsAsync(Guid.Parse(userId)));
        }

        [HttpGet]
        public async Task<IActionResult> GetGuilds()
        {
            return Ok(await _guildService.GetAllAsync());
        }

        [HttpGet("regions")]
        public async Task<IActionResult> GetRegions()
        {
            return Ok(await _botService.GetRegionsAsync());
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateGuild(GuildDto guildDto)
        {
            var result = await _botService.EditGuildAsync(guildDto, await _userManager.GetUserAsync(User));

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("{id}/bans")]
        public async Task<IActionResult> GetBans([FromRoute] string id)
        {
            return Ok(await _botService.GetBansAsync(id, await _userManager.GetUserAsync(User)));
        }

        [HttpGet("{id}/tempbans")]
        public async Task<IActionResult> GetTemporaryBans([FromRoute] string id)
        {
            return Ok(await _guildService.GetTemporaryBansAsync(id, await _userManager.GetUserAsync(User)));
        }

        [HttpPost("{id}/tempbans")]
        public async Task<IActionResult> AddTemporaryBans([FromRoute] string id, List<TemporaryBanDto> temporaryBanDto)
        {
            try
            {
                await _guildService.AddTemporaryBansAsync(id, temporaryBanDto, await _userManager.GetUserAsync(User));
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return BadRequest();
        }

        [HttpDelete("{id}/tempbans")]
        public async Task<IActionResult> RemoveTemporaryBans([FromRoute] string id, List<TemporaryBanDto> temporaryBanDto)
        {
            try
            {
                await _guildService.RemoveTemporaryBansAsync(id, temporaryBanDto, await _userManager.GetUserAsync(User));
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return BadRequest();
        }
    }
}