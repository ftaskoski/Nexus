using Microsoft.AspNetCore.Mvc;
using Nexus.Data;
using Nexus.DTOS;
using Nexus.Interfaces;
using Nexus.Models;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendRequestsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ISystemUser _systemUser;


        public FriendRequestsController(AppDbContext dbContext, ISystemUser systemUser)
        {
            _dbContext = dbContext;
            _systemUser = systemUser;
        }


        [HttpGet("{username}")]
        public ActionResult<IEnumerable<UserSummaryDto>> SearchFriends(string username)
        {
            var usersWithStatus = _dbContext.Users
                .Where(x => x.Username.StartsWith(username) && x.Id != _systemUser.Id)
             .Select(u => new UserSummaryDto
             {
                 Id = u.Id,
                 Username = u.Username,
                 Status = (FriendRequestStatus?)_dbContext.FriendRequests
        .Where(fr => (fr.SenderId == _systemUser.Id && fr.ReceiverId == u.Id) ||
                     (fr.ReceiverId == _systemUser.Id && fr.SenderId == u.Id))
        .Select(fr => (int?)fr.Status)
        .FirstOrDefault(),
                 IsIncoming = _dbContext.FriendRequests
        .Any(fr => fr.ReceiverId == _systemUser.Id && fr.SenderId == u.Id)
             })

                .ToList();

            if (usersWithStatus.Count == 0)
            {
                return NotFound($"User with username '{username}' not found.");
            }

            return Ok(usersWithStatus);
        }


        [HttpPost("add-friend")]
        public IActionResult SendFriendRequest([FromBody] FriendRequestDto friendRequestDto)
        {
            if (friendRequestDto == null || friendRequestDto.ReceiverId == Guid.Empty)
            {
                return BadRequest(new { message = "Invalid data" });
            }

            var existingRequest = _dbContext.FriendRequests.FirstOrDefault(fr =>
                (fr.SenderId == _systemUser.Id && fr.ReceiverId == friendRequestDto.ReceiverId) ||
                (fr.ReceiverId == _systemUser.Id && fr.SenderId == friendRequestDto.ReceiverId));

            if (existingRequest != null)
            {
                return BadRequest(new { message = "Friend request already exists" });
            }

            var friendRequest = new FriendRequestModel
            {
                Id = Guid.NewGuid(),
                SenderId = _systemUser.Id,
                ReceiverId = friendRequestDto.ReceiverId,
                Status = FriendRequestStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _dbContext.FriendRequests.Add(friendRequest);
            _dbContext.SaveChanges();

            return Ok(new { message = "Friend request sent" });
        }

    }
}
