using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.UserService
{
    public class UpdateReqDto
    {
        public string? Full_name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }
    }
}
