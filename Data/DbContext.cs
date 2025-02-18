using Microsoft.EntityFrameworkCore;
using Nexus.Models;
using System.Collections.Generic;

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

    }
}
