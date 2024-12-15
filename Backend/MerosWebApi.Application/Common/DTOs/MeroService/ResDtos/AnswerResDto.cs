using MerosWebApi.Core.Models.PhormAnswer;

namespace MerosWebApi.Application.Common.DTOs.MeroService.ResDtos
{
    public class AnswerResDto
    {
        public string QuestionText { get; set; }

        public List<string> QuestionAnswers { get; set; }

        public static AnswerResDto Map(Answer answer)
        {
            return new AnswerResDto
            {
                QuestionAnswers = answer.QuestionAnswers,
                QuestionText = answer.QuestionText
            };
        }
    }
}
