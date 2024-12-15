using System.Globalization;

namespace MerosWebApi.Core.Models.Exceptions
{
    public class FieldException : CoreException
    {
        public FieldException()
        {
        }

        public FieldException(string message) : base(message)
        {
        }

        public FieldException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
