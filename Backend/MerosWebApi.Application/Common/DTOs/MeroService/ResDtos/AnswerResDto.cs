using MerosWebApi.Core.Models.PhormAnswer;

namespace MerosWebApi.Application.Common.DTOs.MeroService.ResDtos
{
    public class AnswerResDto
    {
        public string QuestionTitle { get; set; }

        public string? QuestionAnswer { get; set; }

        public static AnswerResDto Map(Answer answer)
        {
            return new AnswerResDto
            {
                QuestionAnswer = answer.QuestionAnswer,
                QuestionTitle = answer.QuestionText
            };
        }
    }
}
