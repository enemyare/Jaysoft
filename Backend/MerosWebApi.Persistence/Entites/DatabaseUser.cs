using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using String = System.String;

namespace MerosWebApi.Persistence.Entites
{
    public class DatabaseUser
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("full_name")]
        [BsonRequired]
        public string Full_name { get; set; }

        [BsonElement("email")]
        [BsonRequired]
        public string? Email { get; set; }

        [BsonElement("created_at")]
        [BsonRequired]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [BsonElement("last_login_at")]
        public DateTime? LastLoginAt { get; set; }

        [BsonElement("login_fail_at")]
        public DateTime? LoginFailedAt { get; set; }

        [BsonElement("login_fail_count")]
        public int LoginFailedCount { get; set; }

        [BsonElement("unconf_email")]
        public string? UnconfirmedEmail { get; set; }

        [BsonElement("unconf_email_cr_at")]
        public DateTime? UnconfirmedEmailCreatedAt { get; set; }

        [BsonElement("unconf_email_code")]
        public string? UnconfirmedEmailCode { get; set; }

        [BsonElement("unconf_email_count")]
        public int UnconfirmedEmailCount { get; set; }

        [BsonElement("reset_pwd_cr_at")]
        public DateTime? ResetPasswordCreatedAt { get; set; }

        [BsonElement("reset_pwd_count")]
        public int ResetPasswordCount { get; set; }

        [BsonElement("reset_pwd_code")]
        public string? ResetPasswordCode { get; set; }

        [BsonElement("pwd_hash")]
        public byte[] PasswordHash { get; set; }

        [BsonElement("pwd_salt")]
        public byte[] PasswordSalt { get; set; }

        [BsonElement("refresh_token")]
        public string? RefreshToken { get; set; }

        [BsonElement("refr_token_cr_at")]
        public DateTime TokenCreated { get; set; }

        [BsonElement("refr_token_expires")]
        public DateTime TokenExpires { get; set; }

        [BsonElement("is_active")]
        public bool IsActive { get; set; }
    }
}
