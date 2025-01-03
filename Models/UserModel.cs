using System;

namespace Nexus.Models
{
    public class UserModel
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 
        public string Email { get; set; } = string.Empty; 
        public string PasswordHash { get; set; } = string.Empty; 
        public string Salt { get; set; } = string.Empty; 
        public string Username { get; set; } = string.Empty; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
    }
}
