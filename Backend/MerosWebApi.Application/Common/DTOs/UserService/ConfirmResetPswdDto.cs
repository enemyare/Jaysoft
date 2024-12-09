using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.User;

namespace MerosWebApi.Application.Common.DTOs.UserService
{
    public class ConfirmResetPswdDto
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public static ConfirmResetPswdDto CreateFromUser(User user)
        {
            var dto = new ConfirmResetPswdDto();

            dto.Id = user.Id;

            dto.FullName = user.Full_name;

            dto.Email = user.Email;

            return dto;
        }
    }
}
