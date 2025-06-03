using Microsoft.AspNetCore.Mvc;
using Nexus.Models;

namespace Nexus.Interfaces
{
    public interface IFriendsRepository : IRepository<FriendModel>
    {
        Task<object> GetFriendsAsync(Guid userId);
        Task<FriendModel?> GetFriend(Guid id);
        Task<object> GetRecentChatsAsync(Guid userId);
    }
}
