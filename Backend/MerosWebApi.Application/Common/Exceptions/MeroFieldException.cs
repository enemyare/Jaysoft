using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Application.Common.DTOs.MeroService;

namespace MerosWebApi.Application.Common.Exceptions
{
    public class MeroFieldException : AppException
    {
        public FieldReqDto FieldReqDto { get; set; }

        public string Message { get; set; }
        public MeroFieldException()
        {
        }

        public MeroFieldException(FieldReqDto dto, string message) : base(message)
        {
            FieldReqDto = dto;
            Message = message;
        }

        public MeroFieldException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
