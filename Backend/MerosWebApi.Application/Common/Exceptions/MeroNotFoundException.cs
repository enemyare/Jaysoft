using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.Exceptions
{
    public class MeroNotFoundException : AppException
    {
        public MeroNotFoundException()
        {
        }

        public MeroNotFoundException(string message) : base(message)
        {
        }

        public MeroNotFoundException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
