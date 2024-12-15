using MerosWebApi.Application.Common.SecurityHelpers;

namespace MerosWebApi.Application.Interfaces
{
    public interface ITokenGenerator
    {
        public string GenerateAccessToken(string userId);

        public RefreshToken GenerateRefreshToken();
    }
}
