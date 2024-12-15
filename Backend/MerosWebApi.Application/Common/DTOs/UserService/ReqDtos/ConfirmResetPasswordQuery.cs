namespace MerosWebApi.Application.Common.DTOs.UserService.ReqDtos
{
    public class ConfirmResetPasswordQuery
    {
        public string Code { get; set; }
        public string Email { get; set; }
    }
}
