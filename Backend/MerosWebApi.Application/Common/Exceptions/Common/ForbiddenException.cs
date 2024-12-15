using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.Common
{
    public class ForbiddenException : AppException
    {
        public ForbiddenException()
        {
        }

        public ForbiddenException(string message) : base(message)
        {
        }

        public ForbiddenException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}