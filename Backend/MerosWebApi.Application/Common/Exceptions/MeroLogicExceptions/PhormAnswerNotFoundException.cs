using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.MeroLogicExceptions
{
    public class PhormAnswerNotFoundException : AppException
    {
        public PhormAnswerNotFoundException()
        {
        }

        public PhormAnswerNotFoundException(string message) : base(message)
        {
        }

        public PhormAnswerNotFoundException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
