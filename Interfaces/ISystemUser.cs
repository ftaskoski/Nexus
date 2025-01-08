namespace Nexus.Interfaces
{
    public interface ISystemUser
    {
        string? Email { get; }
        Guid Id { get; }
        bool IsAuthenticated { get; }
        string? Username { get; }
    }
}