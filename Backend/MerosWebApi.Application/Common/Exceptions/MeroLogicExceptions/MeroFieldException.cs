using MerosWebApi.Application.Common.DTOs.MeroService;
using System.Globalization;

namespace MerosWebApi.Application.Common.Exceptions.MeroLogicExceptions
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
