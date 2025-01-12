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

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteNotification(string id)
        {
            var notificationId = new Guid(id);
            var notif = _dbContext.FriendRequests.FirstOrDefault(x => x.Id == notificationId);

            _dbContext.FriendRequests.Remove(notif!);
            _dbContext.SaveChanges();

            return Ok();

        }


    }
}
