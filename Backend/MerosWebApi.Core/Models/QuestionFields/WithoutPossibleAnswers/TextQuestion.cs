using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.QuestionFields.WithoutPossibleAnswers
{
    public class TextQuestion : WithoutPossibleAnswerQuestion
    {
        public TextQuestion(string text, bool required, List<string> answers)
            : base(text, "text", required, answers)
        {
        }
    }
}
