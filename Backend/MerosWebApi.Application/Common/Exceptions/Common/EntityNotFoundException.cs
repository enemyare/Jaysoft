using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.Common
{
    public class EntityNotFoundException : AppException
    {
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
