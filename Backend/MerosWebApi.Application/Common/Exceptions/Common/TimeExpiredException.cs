using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.Common
{
    public class TimeExpiredException : AppException
    {
        public TimeExpiredException()
        {
        }

        public TimeExpiredException(string message) : base(message)
        {
        }

        public TimeExpiredException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
