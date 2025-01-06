namespace MerosWebApi.Core.Models.PhormAnswer
{
    public class Answer
    {
        public string QuestionText { get; }

        public string? QuestionAnswer { get; }

        public Answer(string text, string answers)
        {
            QuestionText = text;
            QuestionAnswer = answers;
        }
    }
}
