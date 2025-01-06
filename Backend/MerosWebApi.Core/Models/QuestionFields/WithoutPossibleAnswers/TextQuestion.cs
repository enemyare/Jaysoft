using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.QuestionFields.WithoutPossibleAnswers
{
    public class TextQuestion : WithoutPossibleAnswerQuestion
    {
        public TextQuestion(string title)
            : base(title, "text")
        {
        }
    }
}
