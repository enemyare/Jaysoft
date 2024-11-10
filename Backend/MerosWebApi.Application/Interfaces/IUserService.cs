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
        public Task<bool> DeleteAsync(Guid id, Guid userId);
        public Task<GetDetailsResDto> GetDetailsAsync(Guid id);
        public Task PasswordResetAsync(PasswordResetDto dto);
        public Task<GetDetailsResDto> RegisterAsync(RegisterReqDto dto);
        public Task<GetDetailsResDto> UpdateAsync(Guid id, Guid userId, UpdateReqDto dto);
    }
}