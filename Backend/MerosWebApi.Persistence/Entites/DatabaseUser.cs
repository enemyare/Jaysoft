using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MerosWebApi.Persistence.Entites
{
    public class DatabaseUser
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("full_name")]
        public string Full_name { get; set; }

        [BsonElement("email")]
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

        [BsonElement("verif_code")]
        public string? VerificationCode { get; set; }

        [BsonElement("verif_code_cr_at")]
        public DateTime? VerificationCodeCreatedAt { get; set; }

        [BsonElement("verif_code_count")]
        public int VerificationCodeCount { get; set; }

        //[BsonElement("last_verif_code_cr_at")]
        //public DateTime? LastVerificationCodeCreateAt { get; set; }

        [BsonElement("refresh_token")]
        public string? RefreshToken { get; set; }

        [BsonElement("refr_token_cr_at")]
        public DateTime TokenCreated { get; set; }

        [BsonElement("refr_token_expires")]
        public DateTime TokenExpires { get; set; }
    }
}
