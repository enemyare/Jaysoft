using MerosWebApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.UserService
{
    public class AuthenticationResDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
        
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
