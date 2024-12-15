using MerosWebApi.Application.Common.DTOs.MeroService;

namespace MerosWebApi.Application.Common.Exceptions.MeroLogicExceptions
{
    public class MeroTimeException : AppException
    {
        public TimePeriodsReqDto TimePeriodsReqDto { get; set; }

        public MeroTimeException(TimePeriodsReqDto dto, string message) : base(message)
        {
            TimePeriodsReqDto = dto;
        }
    }
}
