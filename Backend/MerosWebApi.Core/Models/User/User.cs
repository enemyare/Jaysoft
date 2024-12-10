using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.User
{
    public class User
    {
        public string Id { get; set; }

        public string Full_name { get; set; }

        public string? Email { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public DateTime? LoginFailedAt { get; set; }

        public int LoginFailedCount { get; set; }

        public string? UnconfirmedEmail { get; set; }

        public DateTime? UnconfirmedEmailCreatedAt { get; set; }

        public string? UnconfirmedEmailCode { get; set; }

        public int UnconfirmedEmailCount { get; set; }

        public string? VerificationCode { get; set; }

        public DateTime? VerificationCodeCreatedAt { get; set; }

        //public DateTime? LastVerificationCodeCreateAt { get; set; }

        public int VerificationCodeCount { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime TokenCreated { get; set; }

        public DateTime TokenExpires { get; set; }
    }
}
