using MerosWebApi.Core.Models.PhormAnswer;

namespace MerosWebApi.Application.Common.DTOs.MeroService.ResDtos
{
    public class PhormAnswerResDto
    {
        public string Id { get; set; }

        public string MeroId { get; set; }

        public List<AnswerResDto> Answers { get; set; }

        public TimePeriodsResDto TimePeriod { get; set; }

        public DateTime CreatedTime { get; set; }

        public static PhormAnswerResDto Map(PhormAnswer phormAnswer)
        {
            return new PhormAnswerResDto
            {
                Id = phormAnswer.Id,
                MeroId = phormAnswer.MeroId,
                Answers = phormAnswer.Answers
                    .Select(a => AnswerResDto.Map(a))
                    .ToList(),
                TimePeriod = TimePeriodsResDto.Map(phormAnswer.TimePeriod),
                CreatedTime = phormAnswer.CreatedTime
            };
        }
    }
}
