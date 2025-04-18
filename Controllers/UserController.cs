using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IUserRepository _userRepository;

        public UserController(AppDbContext dbContext,
            IPasswordService passwordService,
            ISystemUser systemUser,
            IHubContext<OnlineUsersHub> hubContext,
            IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _systemUser = systemUser;
            _hubContext = hubContext;
            _userRepository = userRepository;
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
        public async Task<IActionResult> DeleteUserByUsername(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            await _userRepository.DeleteAsync(user.Id);

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

           await _userRepository.AddAsync(newUser);

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
            await _userRepository.UpdateAsync(user);

            var onlineFriends = await _userRepository.GetOnlineFriendsForUser(user.Id);

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

            var user = await _userRepository.GetByIdAsync(_systemUser.Id);
            if (user != null)
            {
                user.IsOnline = false;
                await _userRepository.UpdateAsync(user);

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            if (users == null || !users.Any())
            {
                return NotFound(new { message = "No users found" });
            }
            var onlineUsers = users.Where(u => u.IsOnline).ToList();
            return Ok(users);
        }

    }
}
