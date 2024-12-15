using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.Exceptions.Common
{
    internal class TooManyAttemptsException : AppException
    {
        public TooManyAttemptsException()
        {
        }

        public TooManyAttemptsException(string message) : base(message)
        {
        }

        public TooManyAttemptsException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
