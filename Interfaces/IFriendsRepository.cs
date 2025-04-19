using Nexus.Models;

namespace Nexus.Interfaces
{
    public interface IFriendsRepository : IRepository<FriendModel>
    {
        Task<IEnumerable<FriendModel>> GetFriendsAsync(Guid userId);
        Task<FriendModel?> GetFriend(Guid id);
    }
}
