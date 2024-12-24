using MerosWebApi.Application.Common.SecurityHelpers;

namespace MerosWebApi.Application.Common.DTOs.UserService
{
    public class LogInResult
    {
        public AuthenticationResDto AuthenticationResDto { get; }

        public MyToken AccessToken { get; }

        public MyToken RefreshToken { get; }

        public LogInResult(AuthenticationResDto resDto, MyToken accessToken, MyToken refreshToken)
        {
            AuthenticationResDto = resDto;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
