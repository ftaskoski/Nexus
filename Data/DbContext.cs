using Microsoft.EntityFrameworkCore;
using Nexus.Models;

namespace Nexus.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; } = null!;
        public DbSet<FriendRequestModel> FriendRequests { get; set; } = null!;
        public DbSet<FriendshipModel> Friendships { get; set; } = null !;
        public DbSet<FriendModel> Friends { get; set; } = null !;
        public DbSet<ChatRoomModel> ChatRooms { get; set; } = null!;
        public DbSet<MessageModel> Messages { get; set; } = null!;

    }
}
