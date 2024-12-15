using MerosWebApi.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MerosWebApi.Application.Common.SecurityHelpers
{
    public class PasswordHelper : IPasswordHelper
    {
        public (byte[] passwordHash, byte[] passwordSalt) CreateHash(string password)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Value cannot be empty or whitespace only string.", nameof(password));

            using var hmac = new HMACSHA512();
            return (
                passwordHash: hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                passwordSalt: hmac.Key);
        }

        public bool VerifyHash(string password, byte[] hash, byte[] salt)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(
                    "Value cannot be empty or whitespace only string.", nameof(password));
            if (hash.Length != 64)
                throw new ArgumentException(
                    "Invalid length of password hash (64 bytes expected).", nameof(hash));
            if (salt.Length != 128)
                throw new ArgumentException(
                    "Invalid length of password salt (128 bytes expected).", nameof(salt));

            using var hmac = new HMACSHA512(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            for (var i = 0; i < computedHash.Length; i++)
                if (computedHash[i] != hash[i])
                    return false;

            return true;
        }

        public string GenerateRandomString(int length)
        {
            return new string(Enumerable
                .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length)
                .Select(x =>
                {
                    var cryptoResult = new byte[4];
                    using (var cryptoProvider = RandomNumberGenerator.Create())
                        cryptoProvider.GetBytes(cryptoResult);
                    return x[new Random(BitConverter.ToInt32(cryptoResult, 0)).Next(x.Length)];
                })
                .ToArray());
        }
    }
}
