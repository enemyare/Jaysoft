using System.Globalization;

namespace MerosWebApi.Core.Models.Exceptions
{
    public class NotValidTimePeriodException : CoreException
    {
        public NotValidTimePeriodException()
        {
        }

        public NotValidTimePeriodException(string message) : base(message)
        {
        }

        public NotValidTimePeriodException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
