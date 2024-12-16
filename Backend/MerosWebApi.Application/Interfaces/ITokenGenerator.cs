using MerosWebApi.Application.Common.SecurityHelpers;

namespace MerosWebApi.Application.Interfaces
{
    public interface ITokenGenerator
    {
        public MyToken GenerateAccessToken(string userId);

        public MyToken GenerateRefreshToken();
    }
}
