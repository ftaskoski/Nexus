using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Nexus.Services
{
    public class PasswordService
    {
        public string HashPassword(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var hashBytes = KeyDerivation.Pbkdf2(
                password: password, 
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(hashBytes);
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
