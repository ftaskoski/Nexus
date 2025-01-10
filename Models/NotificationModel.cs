namespace Nexus.Models
{
    public class NotificationModel
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public Guid SenderId { get; set; }
        public string SenderName { get; set; }
    }

}
