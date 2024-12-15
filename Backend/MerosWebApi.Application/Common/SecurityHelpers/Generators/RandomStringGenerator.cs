using System.Security.Cryptography;

namespace MerosWebApi.Application.Common.SecurityHelpers.Generators
{
    public static class RandomStringGenerator
    {
        public static string GenerateRandomString(int length)
        {
            return new string(Enumerable
                .Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
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
