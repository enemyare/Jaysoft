using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.SecurityHelpers;

namespace MerosWebApi.Application.Interfaces
{
    public interface IUserService
    {
        public Task<AuthenticationResDto> AuthenticateAsync(AuthenticateReqDto dto);
        public Task ConfirmEmailAsync(string code);
        public Task<string> RefreshAccessToken(string RefreshToken);
        public Task<ConfirmResetPswdDto> ConfirmResetPasswordAsync(string code, string email);
        public Task<bool> DeleteAsync(string id, string userId);
        public Task<GetDetailsResDto> GetDetailsAsync(string id);
        public Task PasswordResetAsync(PasswordResetDto dto);
        public Task<GetDetailsResDto> RegisterAsync(RegisterReqDto dto);
        public Task<GetDetailsResDto> UpdateAsync(string id, string userId, UpdateReqDto dto);
    }
}