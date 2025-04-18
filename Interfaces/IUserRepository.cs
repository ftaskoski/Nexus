using Nexus.DTOS;
using Nexus.Models;
using Nexus.Repositories;

namespace Nexus.Interfaces
{
    public interface IUserRepository  : IRepository<UserModel>
    {
        Task<List<OnlineUserDto>> GetOnlineFriendsForUser(Guid userId);
    }
}