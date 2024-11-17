using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Application.Common.DTOs.MeroService;

namespace MerosWebApi.Application.Common.Exceptions
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
