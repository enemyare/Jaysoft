using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.Common
{
    public class NotCreatedException : AppException
    {
        public NotCreatedException()
        {
        }

        public NotCreatedException(string message) : base(message)
        {
        }

        public NotCreatedException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
