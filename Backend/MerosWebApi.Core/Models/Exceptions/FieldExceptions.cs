using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.Exceptions
{
    public class FieldException : Exception
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
