using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Nexus.Data;
using Nexus.Hubs;
using Nexus.Interfaces;
using Nexus.Models;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {

        private readonly AppDbContext _dbContext;
        private readonly ISystemUser _systemUser;
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly IUserRepository _userRepository;


        public FriendsController(AppDbContext dbContext, 
            ISystemUser systemUser, 
            IHubContext<NotificationsHub> hubContext,
            IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _systemUser = systemUser;
            _hubContext = hubContext;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IEnumerable<FriendModel> GetFriends()
        {
            var currentUserId = _systemUser.Id;

            var friends = _dbContext.Friendships
                .Where(f => f.User1Id == currentUserId || f.User2Id == currentUserId)
                .Join(
                    _dbContext.Users,
                    friendship => friendship.User1Id == currentUserId ? friendship.User2Id : friendship.User1Id,
                    user => user.Id,
                    (friendship, user) => new FriendModel
                    {
                        Id = user.Id,
                        Username = user.Username,
                        IsOnline = user.IsOnline,

                    })
                .ToList();

            return friends;
        }


    }
}
