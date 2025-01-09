using Nexus.Models;

namespace Nexus.DTOS
{
    public class UserSummaryDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public FriendRequestStatus? Status { get; set; }

    }

}
