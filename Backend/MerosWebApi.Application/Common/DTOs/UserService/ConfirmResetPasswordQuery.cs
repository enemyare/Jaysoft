using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.UserService
{
    public class ConfirmResetPasswordQuery
    {
        public string Code { get; set; }
        public string Email { get; set; }
    }
}
