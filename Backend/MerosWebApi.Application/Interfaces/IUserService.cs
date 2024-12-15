using MerosWebApi.Application.Common.DTOs.MeroService;
using MerosWebApi.Application.Common.DTOs.UserService;

namespace MerosWebApi.Application.Interfaces
{
    public interface IUserService
    {
        public Task<LogInResult> LogInAsync(string authCode);

        public Task<string> RefreshAccessToken(string RefreshToken);

        public Task<bool> DeleteAsync(string id, string userId);

        public Task<GetDetailsResDto> GetDetailsAsync(string id);

        public Task<GetDetailsResDto> UpdateAsync(string id, string userId, UpdateReqDto dto);

        public Task<UserStatisticResDto> GetUserStatisticAsync(string userId);

        public Task ConfirmEmailAsync(string code);

        public Task<bool> SendUserUniqueInviteCode(string email);
    }
}