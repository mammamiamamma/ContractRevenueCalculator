using System.Security.Cryptography;
using APBDProject.Service.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace APBDProject.Service;

public class SecurityService : ISecurityService
{
    public Tuple<string, string> GetHashedPasswordAndSalt(string password)
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        { 
            rng.GetBytes(salt);
        }
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        string saltBase64 = Convert.ToBase64String(salt);
        return new(hashed, saltBase64);
    }

}