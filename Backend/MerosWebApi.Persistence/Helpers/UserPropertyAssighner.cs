using MerosWebApi.Core.Models.User;
using MerosWebApi.Persistence.Entites;
using System.Linq.Expressions;
using System.Reflection;

namespace MerosWebApi.Persistence.Helpers
{
    /// <summary>
    /// Присваивает значения свойств с одинаковым названием 
    /// из одного объекта другому.
    /// </summary>
    /// <typeparam name="TObjTo">Объект, которому надо присвоить значения свойств.</typeparam>
    /// <typeparam name="TObjFrom">Объект, с которого надо считать свойства.</typeparam>
    public class UserPropertyAssighner 
        : IPropertyAssigner<DatabaseUser, User>,
        IPropertyAssigner<User, DatabaseUser>,
        IPropertyValuesAssigner<DatabaseUser, User>

    {
        public static User MapFrom(DatabaseUser source)
        {
            return new User
            {
                Id = source.Id,
                Full_name = source.Full_name,
                Email = source.Email,
                CreatedAt = source.CreatedAt,
                UpdatedAt = source.UpdatedAt,
                LastLoginAt = source.LastLoginAt,
                LoginFailedAt = source.LoginFailedAt,
                LoginFailedCount = source.LoginFailedCount,
                UnconfirmedEmail = source.UnconfirmedEmail,
                UnconfirmedEmailCreatedAt = source.UnconfirmedEmailCreatedAt,
                UnconfirmedEmailCode = source.UnconfirmedEmailCode,
                UnconfirmedEmailCount = source.UnconfirmedEmailCount,
                VerificationCode = source.VerificationCode,
                VerificationCodeCreatedAt = source.VerificationCodeCreatedAt,
                VerificationCodeCount = source.VerificationCodeCount,
                RefreshToken = source.RefreshToken,
                TokenCreated = source.TokenCreated,
                TokenExpires = source.TokenExpires,
            };
        }

        public static DatabaseUser MapFrom(User source)
        {
            return new DatabaseUser
            {
                Id = source.Id,
                Full_name = source.Full_name,
                Email = source.Email,
                CreatedAt = source.CreatedAt,
                UpdatedAt = source.UpdatedAt,
                LastLoginAt = source.LastLoginAt,
                LoginFailedAt = source.LoginFailedAt,
                LoginFailedCount = source.LoginFailedCount,
                UnconfirmedEmail = source.UnconfirmedEmail,
                UnconfirmedEmailCreatedAt = source.UnconfirmedEmailCreatedAt,
                UnconfirmedEmailCode = source.UnconfirmedEmailCode,
                UnconfirmedEmailCount = source.UnconfirmedEmailCount,
                VerificationCode = source.VerificationCode,
                VerificationCodeCreatedAt = source.VerificationCodeCreatedAt,
                //LastVerificationCodeCreateAt = source.LastVerificationCodeCreateAt,
                VerificationCodeCount = source.VerificationCodeCount,
                RefreshToken = source.RefreshToken,
                TokenCreated = source.TokenCreated,
                TokenExpires = source.TokenExpires,
            };
        }

        public static void AssignPropertyValues(DatabaseUser to, User from)
        {
            to.Id = from.Id;
            to.Full_name = from.Full_name;
            to.Email = from.Email;
            to.CreatedAt = from.CreatedAt;
            to.UpdatedAt = from.UpdatedAt;
            to.LastLoginAt = from.LastLoginAt;
            to.LoginFailedAt = from.LoginFailedAt;
            to.LoginFailedCount = from.LoginFailedCount;
            to.UnconfirmedEmail = from.UnconfirmedEmail;
            to.UnconfirmedEmailCreatedAt = from.UnconfirmedEmailCreatedAt;
            to.UnconfirmedEmailCode = from.UnconfirmedEmailCode;
            to.UnconfirmedEmailCount = from.UnconfirmedEmailCount;
            to.VerificationCode = from.VerificationCode;
            to.VerificationCodeCreatedAt = from.VerificationCodeCreatedAt;
            to.VerificationCodeCount = from.VerificationCodeCount;
            //to.LastVerificationCodeCreateAt = from.LastVerificationCodeCreateAt;
            to.RefreshToken = from.RefreshToken;
            to.TokenCreated = from.TokenCreated;
            to.TokenExpires = from.TokenExpires;
        }
    }
}
