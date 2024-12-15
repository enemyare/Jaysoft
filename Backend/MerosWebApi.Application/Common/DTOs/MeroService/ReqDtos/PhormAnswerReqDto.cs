using MerosWebApi.Application.Common.DTOs.MeroService.ReqDtos;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class PhormAnswerReqDto
    {
        public string MeroId { get; set; }

        public List<AnswerReqDto> Answers { get; set; }

        public string TimePeriodId { get; set; }
    }
}
