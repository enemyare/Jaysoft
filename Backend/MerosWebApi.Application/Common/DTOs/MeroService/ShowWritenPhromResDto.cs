using MerosWebApi.Core.Models.PhormAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService
{
    public class ShowWritenPhromResDto
    {
        public List<AnswerResDto> Answers { get; set; }

        public TimePeriodShortResDto TimePeriod { get; set; }

        public DateTime CreatedTime { get; set; }

        public static ShowWritenPhromResDto Map(PhormAnswer phormAnswer)
        {
            return new ShowWritenPhromResDto
            {
                Answers = phormAnswer.Answers
                    .Select(a => AnswerResDto.Map(a))
                    .ToList(),
                TimePeriod = TimePeriodShortResDto.Map(phormAnswer.TimePeriod),
            };
        }
    }
}
