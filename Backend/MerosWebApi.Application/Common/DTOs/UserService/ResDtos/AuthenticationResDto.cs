using MerosWebApi.Core.Models.User;

namespace MerosWebApi.Application.Common.DTOs.UserService
{
    public class AuthenticationResDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public static AuthenticationResDto Map(User from)
        {
            return new AuthenticationResDto
            {
                Id = from.Id,
                FullName = from.Full_name,
                Email = from.Email
            };
        }
    }

}
