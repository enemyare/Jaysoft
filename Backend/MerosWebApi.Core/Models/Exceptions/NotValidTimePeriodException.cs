using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.Exceptions
{
    public class NotValidTimePeriodException : Exception
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
