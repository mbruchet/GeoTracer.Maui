using System.Security.Cryptography;
using System.Text;

namespace GeoTracer.Shared.Services;

public record HashedPasswordResult(string Hash, string Salt);

public class PasswordHasher
{

    public HashedPasswordResult HashPassword(string password)
    {
        byte[] saltBytes = GenerateSalt();
        string salt = Convert.ToBase64String(saltBytes);

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
        byte[] hashBytes = SHA256.Create().ComputeHash(passwordBytes);
        string hash = Convert.ToBase64String(hashBytes);

        return new HashedPasswordResult(hash, salt);
    }

    public bool VerifyPassword(string enteredPassword, string salt, string storedHash)
    {
        byte[] passwordBytes = Encoding.UTF8.GetBytes(enteredPassword + salt);
        byte[] hashBytes = SHA256.Create().ComputeHash(passwordBytes);
        string hash = Convert.ToBase64String(hashBytes);
        return hash.Equals(storedHash);
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
}
