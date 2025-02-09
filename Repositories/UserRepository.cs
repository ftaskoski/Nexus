using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.DTOS;
using Nexus.Interfaces;

namespace Nexus.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<OnlineUserDto>> GetOnlineFriendsForUser(Guid userId)
        {
            return await _dbContext.Friendships
                .Where(f => (f.User1Id == userId || f.User2Id == userId) && f.User1Id != f.User2Id)
                .Join(
                    _dbContext.Users.Where(u => u.IsOnline),
                    f => f.User1Id == userId ? f.User2Id : f.User1Id,
                    u => u.Id,
                    (f, u) => new OnlineUserDto
                    {
                        Id = u.Id,
                        Username = u.Username,
                        IsOnline = u.IsOnline
                    })
                .ToListAsync();
        }

    }
}
