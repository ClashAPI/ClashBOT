using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/v1/logs")]
    public class LogsController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogService _logService;

        public LogsController(UserManager<User> userManager, ILogService logService)
        {
            _userManager = userManager;
            _logService = logService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs()
        {
            var logs = await _logService.GetAllAsync();

            return Ok(logs.OrderByDescending(e => e.Date).ToList());
        }

        [HttpGet("guild/{id}")]
        public async Task<IActionResult> GetGuildLogs([FromRoute] string id)
        {
            var logs = await _logService.GetAllByGuildIdAsync(id);

            return Ok(logs.OrderByDescending(e => e.Date).ToList());
        }

        // TODO: Only guild owner
        [HttpDelete("guild/{id}")]
        public async Task<IActionResult> DeleteGuildLogs([FromRoute] string id)
        {
            var user = await _userManager.GetUserAsync(User);
            var logs = await _logService.DeleteAllByGuildIdAsync(id, user);

            return Ok(logs.OrderByDescending(e => e.Date).ToList());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogEntry([FromRoute] string id)
        {
            var result = await _logService.DeleteByIdAsync(Guid.Parse(id));

            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}