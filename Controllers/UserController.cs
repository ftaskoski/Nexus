using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.DTOS;
using Nexus.Hubs;
using Nexus.Interfaces;
using Nexus.Models;
using System.Security.Claims;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly ISystemUser _systemUser;
        private readonly IHubContext<OnlineUsersHub> _hubContext;

        public UserController(AppDbContext dbContext,
            IPasswordService passwordService,
            ISystemUser systemUser,
            IHubContext<OnlineUsersHub> hubContext)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _systemUser = systemUser;
            _hubContext = hubContext;
        }

        public ActionResult<UserModel> GetUserByUsername(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(user);
        }

        [HttpDelete("{username}")]
        public IActionResult DeleteUserByUsername(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();

            return Ok(new { message = "User deleted successfully" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var existingUser = _dbContext.Users
                .FirstOrDefault(u => u.Email == userDto.Email || u.Username == userDto.Username);

            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email or username already exists" });
            }

            var salt = _passwordService.GenerateSalt(32);
            var hashedPassword = _passwordService.HashPassword(userDto.Password, salt);

            var newUser = new UserModel
            {
                Id = Guid.NewGuid(),
                Email = userDto.Email,
                Username = userDto.Username,
                PasswordHash = hashedPassword,
                Salt = salt,
                CreatedAt = DateTime.Now
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, newUser.Username),
                new(ClaimTypes.Email, newUser.Email),
                new("UserId", newUser.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return CreatedAtAction(nameof(GetUserByUsername), new { username = newUser.Username }, new { message = "Account created and logged in" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { message = "Invalid login data" });
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.Email.ToLower() == loginDto.Email.ToLower());

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            var hashedPassword = _passwordService.HashPassword(loginDto.Password, user.Salt);

            if (user.PasswordHash != hashedPassword)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            user.IsOnline = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

            var onlineFriends = await GetOnlineFriendsForUser(user.Id);

            foreach (var friend in onlineFriends)
            {
                await _hubContext.Clients.Group(friend.Id.ToString())
                    .SendAsync("UserStatusChanged", user.Id.ToString(), user.Username, true);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Username),
                new(ClaimTypes.Email, user.Email),
                new("UserId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            return Ok(new { message = "Login successful" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!_systemUser.IsAuthenticated)
            {
                return Unauthorized(new { message = "User not authenticated" });
            }

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == _systemUser.Id);
            if (user != null)
            {
                user.IsOnline = false;
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("UserStatusChanged", user.Id.ToString(), user.Username, false);
            }

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logout successful" });
        }

        [HttpGet("lookup")]
        public IActionResult Lookup()
        {
            var isAuthenticated = _systemUser.IsAuthenticated;
            var username = _systemUser.Username;

            return Ok(new { isAuthenticated, username });
        }

        [HttpGet("friends/online")]
        [Authorize]
        public async Task<IActionResult> GetOnlineFriends()
        {
            var onlineFriends = await GetOnlineFriendsForUser(_systemUser.Id);
            return Ok(onlineFriends);
        }

        private async Task<List<OnlineUserDto>> GetOnlineFriendsForUser(Guid userId)
        {
            return await _dbContext.Friendships
                .Where(f => (f.User1Id == userId || f.User2Id == userId) && f.User1Id != f.User2Id)
                .Join(
                    _dbContext.Users.Where(u => u.IsOnline),
                    f => f.User1Id == userId ? f.User2Id : f.User1Id,
                    u => u.Id,
                    (f, u) => new OnlineUserDto
                    {
                        Id = u.Id,
                        Username = u.Username,
                        IsOnline = u.IsOnline
                    })
                .ToListAsync();
        }
    }
}
