using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.Questions
{
    public class ShortTextQuestion : Field
    {
        public ShortTextQuestion(string text, bool required, List<string> answers)
            : base(text, nameof(ShortTextQuestion), required)
        {
            if (answers != null)
                throw new FieldException($"{nameof(ShortTextQuestion)} не должно иметь варианты ответов");
        }

        public override List<string> SelectAnswer(params string[] answers)
        {
            if (!Required && answers.Length == 0)
                return answers.ToList();

            if (answers.Length != 1)
                throw new FieldException($"Поле {nameof(ShortTextQuestion)} должено иметь один ответ");

            return new List<string>() { answers[0] };
        }

        public override List<string> Answers { get; }
    }
}
