using Microsoft.EntityFrameworkCore;
using Nexus.Data;
using Nexus.DTOS;
using Nexus.Interfaces;
using Nexus.Models; 

namespace Nexus.Repositories
{
    public class UserRepository : Repository<UserModel>, IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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