using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.Common
{
    public class NotPossibleUpdateException : AppException
    {
        public NotPossibleUpdateException()
        {
        }

        public NotPossibleUpdateException(string message) : base(message)
        {
        }

        public NotPossibleUpdateException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
