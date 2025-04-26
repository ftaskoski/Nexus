namespace Nexus.Models
{
    public class MessageModel
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; } 
        public Guid ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public string ChatRoomId { get; set; } = string.Empty; 

        public MessageModel()
        {
            Id = Guid.NewGuid();
            SentAt = DateTime.UtcNow;
        }
    }
}
