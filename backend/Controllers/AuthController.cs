using backend.DTOs;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IGuildService _guildService;
        private readonly UserManager<User> _userManager;
        private readonly IDiscordDataService _discordDataService;

        public AuthController(UserManager<User> userManager,
            IDiscordDataService discordDataService, IUserService userService,
            IConfiguration configuration, IGuildService guildService)
        {
            _userManager = userManager;
            _discordDataService = discordDataService;
            _userService = userService;
            _configuration = configuration;
            _guildService = guildService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var userDetails = await _discordDataService.GetDiscordUserDetailsAsync(loginDto.AccessToken);
            var userGuilds = await _discordDataService.GetDiscordUserGuildsAsync(loginDto.AccessToken);
            var userId = userDetails.Id;
            var userName = userDetails.Username;
            var userDiscriminator = userDetails.Discriminator;
            var userEmail = userDetails.Email;

            var user =
                await _userService.GetByUsernameAndDiscriminatorAsync(userName, userDiscriminator);

            // User is not stored in our database yet
            if (Equals(user, default(User)))
            {
                var userToRegister = new User
                {
                    Discriminator = userDiscriminator,
                    UserId = userId,
                    UserName = userName,
                    Email = userEmail,
                    // AccessToken = id
                };

                var savedUser = await _userService.AddAsync(userToRegister);

                foreach (var userGuild in userGuilds)
                {
                    if (userGuild.Owner)
                    {
                        await _guildService.AddAsync(new Guild
                        {
                            GuildId = userGuild.Id
                        });
                    }
                }

                await _guildService.SaveAllAsync();

                /*
                if (!await _botService.GetIfUserIsSupertesterAsync(user.UserId))
                {
                    return Forbid();
                }
                */

                return Ok(new
                {
                    token = await GenerateJwtTokenAsync(savedUser, loginDto.AccessToken),
                    user = new { savedUser.Id, savedUser.UserName, savedUser.Email }
                });
            }
            // User is already stored in our database
            else
            {
                // if (user.AccessToken != id) user.AccessToken = id;
                foreach (var userGuild in userGuilds)
                {
                    var guilds = await _guildService.GetAllAsync();

                    if (!guilds.Select(g => g.GuildId).ToList().Contains(userGuild.Id))
                    {
                        if (userGuild.Owner)
                        {
                            await _guildService.AddAsync(new Guild
                            {
                                GuildId = userGuild.Id
                            });
                        }
                    }
                }

                await _guildService.SaveAllAsync();

                /*
                if (!await _botService.GetIfUserIsSupertesterAsync(user.UserId))
                {
                    return Forbid();
                }
                */

                // Check if user is banned or suspended
                if (user.LockoutEnd != null || user.LockoutEnd > DateTime.Now)
                {
                    // If so, we will not let them in, and provide the reason for that
                    return Unauthorized(new { reason = "banned", expiresAt = user.LockoutEnd });
                }

                return Ok(new { token = await GenerateJwtTokenAsync(user, loginDto.AccessToken), user = new { user.Id, user.UserName, user.Email } });
            }

            // return Ok(await GenerateJwtTokenAsync(user));
        }

        /*
        [HttpGet("callback")]
        public async Task<IActionResult> SignIn([FromQuery] string code)
        {
            var token = await _discordDataService.GetDiscordUserAccessTokenAsync(code);
            var accessToken = token["access_token"];
            
            return Redirect($"http://localhost:4200/login/{accessToken}");
        }
        /*

        /*
        // TODO: Authorize user and create a filter for checking token validity
        [HttpPost("login/admin")]
        public async Task<IActionResult> WebSudoLogin()
        {
            var user = await _userManager.GetUserAsync(User);
            var token = await GenerateWebSudoJwtTokenAsync(user);

            return Ok(token);
        }

        private async Task<string> GenerateWebSudoJwtTokenAsync(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim("session_id", Guid.NewGuid().ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        */

        private async Task<string> GenerateJwtTokenAsync(User user, string accessToken)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim("discord_member_id", user.UserId));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("discord_member_discriminator", user.Discriminator));
            claims.Add(new Claim("discord_access_token", accessToken));
            claims.Add(new Claim("is_superuser", user.IsSuperuser.ToString()));

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles) claims.Add(new Claim(ClaimTypes.Role, role));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // Token validity duration
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}