using Nexus.Interfaces;
using System.Security.Claims;

namespace Nexus.Services
{
    public class SystemUser : ISystemUser
    {
        public Guid Id { get; }
        public string? Username { get; }
        public string? Email { get; }
        public bool IsAuthenticated { get; }

        public SystemUser(IHttpContextAccessor httpContextAccessor)
        {
            var userClaims = httpContextAccessor.HttpContext?.User;

            Id = Guid.TryParse(userClaims?.FindFirst("UserId")?.Value, out var userId) ? userId : Guid.Empty;
            Username = userClaims?.FindFirst(ClaimTypes.Name)?.Value;
            Email = userClaims?.FindFirst(ClaimTypes.Email)?.Value;
            IsAuthenticated = userClaims?.Identity?.IsAuthenticated ?? false;
        }
    }
}
