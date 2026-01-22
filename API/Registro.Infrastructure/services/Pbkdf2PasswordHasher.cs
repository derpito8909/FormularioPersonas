using System.Security.Cryptography;
using Registro.Application.Interfaces;

namespace Registro.Infrastructure.services;

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    public (byte[] hash, byte[] salt) Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);
        return (hash, salt);
    }

    public bool Verify(string password, byte[] storedHash, byte[] storedSalt)
    {
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, storedSalt, 100_000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(hash, storedHash);
    }
}