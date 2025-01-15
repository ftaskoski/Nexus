using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Nexus.Data;
using Nexus.DTOS;
using Nexus.Hubs;
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
        private readonly IHubContext<NotificationsHub> _hubContext;


        public FriendRequestsController(AppDbContext dbContext, ISystemUser systemUser, IHubContext<NotificationsHub> hubContext)
        {
            _dbContext = dbContext;
            _systemUser = systemUser;
            _hubContext = hubContext;
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
                    Status = _dbContext.Friendships
                        .Any(f => (f.User1Id == _systemUser.Id && f.User2Id == u.Id) ||
                                  (f.User2Id == _systemUser.Id && f.User1Id == u.Id))
                        ? FriendRequestStatus.Accepted
                        : (FriendRequestStatus?)_dbContext.FriendRequests
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
        public async Task<IActionResult> SendFriendRequest([FromBody] FriendRequestDto friendRequestDto)
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

            var receiverId = friendRequestDto.ReceiverId.ToString();

            try
            {
                await _hubContext.Clients.Group(receiverId)
                    .SendAsync("ReceiveNotification", new NotificationModel
                    {
                        Id = friendRequest.Id,
                        SenderId = _systemUser.Id,
                        Message = "You have a new friend request!",
                        SenderName = _systemUser.Username!,
                        Type = "FriendRequest"
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
            }

            return Ok(new { message = "Friend request sent" });
        }

        [HttpGet("notifications")]
        public IActionResult GetNotifications()
        {
            var notifications = _dbContext.FriendRequests
                .Where(fr => fr.ReceiverId == _systemUser.Id)
                .Select(fr => new NotificationModel
                {
                    Id = fr.Id,
                    SenderId = fr.SenderId,
                    SenderName = _dbContext.Users
                        .Where(u => u.Id == fr.SenderId)
                        .Select(u => u.Username)
                        .FirstOrDefault() ?? "Unknown",
                    Message = fr.Status == FriendRequestStatus.Pending
                        ? "You have a pending friend request."
                        : "Friend request status updated.",
                    Type = "FriendRequest"
                })
                .ToList();

            return Ok(notifications);
        }

        [HttpDelete("decline/{id}")]
        public IActionResult DeleteNotification(string id)
        {
            var notificationId = new Guid(id);
            var notif = _dbContext.FriendRequests.FirstOrDefault(x => x.Id == notificationId);

            _dbContext.FriendRequests.Remove(notif!);
            _dbContext.SaveChanges();

            return Ok();

        }

        [HttpPost("accept/{id}")]
        public IActionResult AcceptFriendRequest(string id)
        {
            var requestId = new Guid(id);
            var friendRequest = _dbContext.FriendRequests
                .FirstOrDefault(fr => fr.Id == requestId && fr.ReceiverId == _systemUser.Id);

            if (friendRequest == null)
            {
                return NotFound("Friend request not found");
            }

            if (friendRequest.Status != FriendRequestStatus.Pending)
            {
                return BadRequest("Friend request has already been processed");
            }

            var existingFriendship = _dbContext.Friendships.FirstOrDefault(f =>
                (f.User1Id == friendRequest.SenderId && f.User2Id == friendRequest.ReceiverId) ||
                (f.User1Id == friendRequest.ReceiverId && f.User2Id == friendRequest.SenderId));

            if (existingFriendship != null)
            {
                return BadRequest("Friendship already exists");
            }

  
      

            var friendship = new FriendshipModel
            {
                Id = Guid.NewGuid(),
                User1Id = friendRequest.SenderId,
                User2Id = friendRequest.ReceiverId,
                CreatedAt = DateTime.Now
            };

            try
            {
                _dbContext.FriendRequests.Remove(friendRequest);
                _dbContext.Friendships.Add(friendship);
                _dbContext.SaveChanges();

                return Ok(new { message = "Friend request accepted" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving friendship: {ex.Message}");
                throw;
            }
        }





    }
}
