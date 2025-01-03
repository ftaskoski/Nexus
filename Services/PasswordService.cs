using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Nexus.Services
{
    public class PasswordService
    {
        public string HashPassword(string password, string salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8)); 
        }

        public string GenerateSalt(int length)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[length];
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
