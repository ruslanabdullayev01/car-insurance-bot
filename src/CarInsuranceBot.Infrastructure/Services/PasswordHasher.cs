using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using CarInsuranceBot.Application.IServices;

namespace CarInsuranceBot.Infrastructure.Services;
public sealed class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        byte[] hash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm))
        {
            hash = pbkdf2.GetBytes(KeySize);
        }

        byte[] dst = new byte[SaltSize + KeySize + 1];
        Buffer.BlockCopy(salt, 0, dst, 1, SaltSize);
        Buffer.BlockCopy(hash, 0, dst, 1 + SaltSize, KeySize);
        return Convert.ToBase64String(dst);
    }

    public bool PasswordMatches(string providedPassword, string passwordHash)
    {
        if (passwordHash == null)
        {
            return false;
        }

        ArgumentNullException.ThrowIfNull(providedPassword);

        byte[] src = Convert.FromBase64String(passwordHash);
        if (src.Length != SaltSize + KeySize + 1 || src[0] != 0)
        {
            return false;
        }

        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(src, 1, salt, 0, SaltSize);
        byte[] storedHash = new byte[KeySize];
        Buffer.BlockCopy(src, 1 + SaltSize, storedHash, 0, KeySize);

        byte[] computedHash;
        using (var pbkdf2 = new Rfc2898DeriveBytes(providedPassword, salt, Iterations, HashAlgorithm))
        {
            computedHash = pbkdf2.GetBytes(KeySize);
        }

        return ByteArraysEqual(storedHash, computedHash);
    }

    [MethodImpl(MethodImplOptions.NoOptimization)]
    private static bool ByteArraysEqual(byte[] a, byte[] b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if (a == null || b == null || a.Length != b.Length)
        {
            return false;
        }

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }

        return true;
    }
}
