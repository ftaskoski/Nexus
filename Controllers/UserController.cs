using Microsoft.AspNetCore.Mvc;
using Nexus.Data;
using Nexus.Models;
using Nexus.Services;  

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly PasswordService _passwordService;

        public UserController(AppDbContext dbContext, PasswordService passwordService)
        {
            _dbContext = dbContext;
            _passwordService = passwordService; 
        }

        [HttpGet("{username}")]
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
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            if (userModel == null)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var existingUser = _dbContext.Users
                .FirstOrDefault(u => u.Email == userModel.Email || u.Username == userModel.Username);

            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email or username already exists" });
            }

            var salt = _passwordService.GenerateSalt(32); 

            var hashedPassword = _passwordService.HashPassword(userModel.PasswordHash, salt); 

            var newUser = new UserModel
            {
                Id = Guid.NewGuid(),
                Email = userModel.Email,
                Username = userModel.Username,
                PasswordHash = hashedPassword,
                Salt = salt, 
                CreatedAt = DateTime.Now
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetUserByUsername), new { username = newUser.Username }, newUser);
        }
    }
}
