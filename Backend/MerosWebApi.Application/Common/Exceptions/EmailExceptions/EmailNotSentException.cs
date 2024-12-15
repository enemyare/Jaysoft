using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.EmailExceptions
{
    public class EmailNotSentException : AppException
    {
        public EmailNotSentException()
        {
        }

        public EmailNotSentException(string message) : base(message)
        {
        }

        public EmailNotSentException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
