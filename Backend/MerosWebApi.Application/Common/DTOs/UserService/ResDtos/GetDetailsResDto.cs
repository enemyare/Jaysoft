using MerosWebApi.Core.Models.User;

namespace MerosWebApi.Application.Common.DTOs.UserService
{
    public class GetDetailsResDto
    {
        public string Id { get; set; }
        public string Full_name { get; set; }

        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public static GetDetailsResDto Map(User user)
        {
            return new GetDetailsResDto
            {
                Id = user.Id,
                Full_name = user.Full_name,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }
}
