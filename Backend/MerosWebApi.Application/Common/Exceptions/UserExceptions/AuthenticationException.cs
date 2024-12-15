using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.UserExceptions
{
    public class AuthenticationException : AppException
    {
        public AuthenticationException()
        {
        }

        public AuthenticationException(string message) : base(message)
        {
        }

        public AuthenticationException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
