using System.ComponentModel.DataAnnotations;

namespace Nexus.Models
{
    public class ChatRoomModel
    {
        [Key]
        public string? ChatRoomId { get; set; }
        public string? User1Id { get; set; }
        public string? User2Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
