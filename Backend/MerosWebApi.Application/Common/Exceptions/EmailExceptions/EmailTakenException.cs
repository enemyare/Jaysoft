using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.EmailExceptions
{
    public class EmailTakenException : AppException
    {
        public EmailTakenException()
        {
        }

        public EmailTakenException(string message) : base(message)
        {
        }

        public EmailTakenException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
