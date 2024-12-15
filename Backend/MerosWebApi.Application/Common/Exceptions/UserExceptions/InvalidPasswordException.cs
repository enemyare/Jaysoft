using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.UserExceptions
{
    public class InvalidPasswordException : AppException
    {
        public InvalidPasswordException()
        {
        }

        public InvalidPasswordException(string message) : base(message)
        {
        }

        public InvalidPasswordException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
