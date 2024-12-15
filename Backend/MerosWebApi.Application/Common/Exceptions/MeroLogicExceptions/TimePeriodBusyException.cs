using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.MeroLogicExceptions
{
    public class TimePeriodBusyException : AppException
    {
        public TimePeriodBusyException()
        {
        }

        public TimePeriodBusyException(string message) : base(message)
        {
        }

        public TimePeriodBusyException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
