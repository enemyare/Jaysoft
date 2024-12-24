using MerosWebApi.Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MerosWebApi.Application.Common.SecurityHelpers.Generators
{
    public class TokensGenerator : ITokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public TokensGenerator(IOptions<JwtOptions> options) =>
            _jwtOptions = options.Value;

        public MyToken GenerateAccessToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);
            var expires = DateTime.Now.AddMinutes(_jwtOptions.ExpiresMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new MyToken
            {
                Token = tokenHandler.WriteToken(token),
                Expires = expires
            };
        }

        public MyToken GenerateRefreshToken()
        {
            var refreshToken = new MyToken
            {
                Token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7)
            };

            refreshToken.Token = refreshToken.Token.Replace('/', 'A');

            return refreshToken;
        }
    }
}
