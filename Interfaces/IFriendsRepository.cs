using Nexus.Models;

namespace Nexus.Interfaces
{
    public interface IFriendsRepository
    {
        Task<IEnumerable<FriendModel>> GetFriendsAsync(Guid userId);
        Task<FriendModel?> GetFriend(Guid id);
    }
}
