namespace Nexus.Interfaces
{
    public interface IPasswordService
    {
        string GenerateSalt(int length);
        string HashPassword(string password, string salt);
    }
}