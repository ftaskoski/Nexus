using Nexus.DTOS;

namespace Nexus.Interfaces
{
    public interface IUserRepository
    {

        Task<List<OnlineUserDto>> GetOnlineFriendsForUser(Guid userId);
    }
}