using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.MeroLogicExceptions
{
    public class PhormAnswerFieldException : AppException
    {
        public PhormAnswerFieldException()
        {
        }

        public PhormAnswerFieldException(string message) : base(message)
        {
        }

        public PhormAnswerFieldException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
