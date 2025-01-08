using Nexus.Interfaces;
using System.Security.Claims;

namespace Nexus.Services
{
    public class SystemUser : ISystemUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SystemUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Id => Guid.Parse(
            _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value
            ?? Guid.Empty.ToString());

        public string? Username =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

        public string? Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}