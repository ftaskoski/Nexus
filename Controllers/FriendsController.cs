using Microsoft.AspNetCore.Mvc;
using Nexus.Interfaces;
using Nexus.Models;

namespace Nexus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendsRepository _friendsRepository;
        private readonly ISystemUser _systemUser;

        public FriendsController(IFriendsRepository friendsRepository,
            ISystemUser systemUser){
            _friendsRepository = friendsRepository;
            _systemUser = systemUser;
        }

        [HttpGet]
        public async Task<IEnumerable<FriendModel>> GetFriends()
        {
            return await _friendsRepository.GetFriendsAsync(_systemUser.Id);

        }

        [HttpGet("{id}")]
        public async Task<FriendModel?> GetFriend(Guid id)
        {
            return await _friendsRepository.GetFriend(id);
        }


    }
}
