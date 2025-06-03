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
        private readonly ISystemUser _systemUser;

        public FriendsRepository(AppDbContext dbContext,ISystemUser systemUser) : base(dbContext)
        {
            _dbContext = dbContext;
            _systemUser = systemUser ?? throw new ArgumentNullException(nameof(systemUser));
        }

        public async Task<object> GetFriendsAsync(Guid userId)
        {
            var friends = await _dbContext.Friendships
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

            var friendsWithMessages = new List<object>();

            foreach (var friend in friends)
            {
                var lastMessage = await _dbContext.Messages
                    .Where(m => (m.SenderId == userId && m.ReceiverId == friend.Id) ||
                               (m.SenderId == friend.Id && m.ReceiverId == userId))
                    .OrderByDescending(m => m.SentAt)
                    .Select(m => new { m.Content, m.SentAt }) 
                    .FirstOrDefaultAsync();

                friendsWithMessages.Add(new
                {
                     friend.Id,
                     friend.Username,
                     friend.IsOnline,
                     lastMessage?.SentAt,
                    LastMessage = lastMessage?.Content ?? "No messages yet",
                });
            }

            return new { friends = friendsWithMessages };
        }

        public async Task<object> GetRecentChatsAsync(Guid userId)
        {
            var friendsWithLastMessage = await _dbContext.Friendships
                .Where(f => f.User1Id == userId || f.User2Id == userId)
                .Join(
                    _dbContext.Users,
                    friendship => friendship.User1Id == userId ? friendship.User2Id : friendship.User1Id,
                    user => user.Id,
                    (friendship, user) => new
                    {
                        User = new FriendModel
                        {
                            Id = user.Id,
                            Username = user.Username,
                            IsOnline = user.IsOnline,
                        },
                        LastMessageTime = _dbContext.Messages
                            .Where(m => (m.SenderId == userId && m.ReceiverId == user.Id) ||
                                         (m.SenderId == user.Id && m.ReceiverId == userId))
                            .Max(m => (DateTime?)m.SentAt)
                    })
                .ToListAsync();
            var recentChats = new List<object>();

            foreach (var friend in friendsWithLastMessage
                .OrderByDescending(f => f.LastMessageTime ?? DateTime.MinValue)
                .Take(5))
            {
                var lastMessage = await _dbContext.Messages
                    .Where(m => (m.SenderId == userId && m.ReceiverId == friend.User.Id) ||
                               (m.SenderId == friend.User.Id && m.ReceiverId == userId))
                    .OrderByDescending(m => m.SentAt)
                    .Select(m => new { m.Content, m.SentAt })
                    .FirstOrDefaultAsync();

                recentChats.Add(new
                {
                    friend.User.Id,
                    friend.User.Username,
                    friend.User.IsOnline,
                    lastMessage?.SentAt,
                    LastMessage = lastMessage?.Content ?? "No messages yet",
                });
            }

            return recentChats;
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
