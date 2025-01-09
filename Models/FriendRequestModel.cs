namespace Nexus.Models
{
    public class FriendRequestModel
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public FriendRequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public UserModel Sender { get; set; }
        public UserModel Receiver { get; set; }
    }
}
