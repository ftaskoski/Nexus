namespace Nexus.Models
{
    public class FriendshipModel
    {
        public Guid Id { get; set; }
        public Guid User1Id { get; set; }
        public Guid User2Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserModel? User1 { get; set; }
        public UserModel? User2 { get; set; }
    }
}
