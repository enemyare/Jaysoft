using System.Globalization;

namespace MerosWebApi.Core.Models.Exceptions
{
    public class FieldTypeException : CoreException
    {
        public FieldTypeException()
        {
        }

        public FieldTypeException(string message) : base(message)
        {
        }

        public FieldTypeException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
