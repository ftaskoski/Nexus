using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.Interfaces;
using Nexus.Models;

namespace Nexus.Repositories
{
    public class FriendsRepository : Repository<FriendModel>,IFriendsRepository
    {
        private readonly AppDbContext _dbContext;

        public FriendsRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<FriendModel>> GetFriendsAsync(Guid userId)
        {
            return await _dbContext.Friendships
                .Where(f => f.User1Id == userId || f.User2Id == userId)
                .Join(
                    _dbContext.Users,
                    friendship => friendship.User1Id == userId ? friendship.User2Id : friendship.User1Id,
                    user => user.Id,
                    (friendship, user) => new FriendModel
                    {
                        Id = user.Id,
                        Username = user.Username,
                        IsOnline = user.IsOnline,
                    })
                .ToListAsync();
        }

        public async Task<FriendModel?> GetFriend(Guid id)
        {
            return await _dbContext.Users
                .Where(u => u.Id == id)
                .Select(user => new FriendModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    IsOnline = user.IsOnline,
                })
                .FirstOrDefaultAsync();
        }

    }
}
