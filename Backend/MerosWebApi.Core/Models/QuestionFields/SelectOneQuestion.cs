using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.Questions
{
    public class SelectOneQuestion : Field, IHavePossibleAnswers
    {
        public SelectOneQuestion(string questionText, bool required, List<string> answers)
            : base(questionText, nameof(SelectOneQuestion), required)
        {
            if (answers == null || answers.Count < 1)
                throw new FieldException($"Поле {nameof(SelectOneQuestion)} должно иметь как " +
                                         $"минимум один вариант ответа");

            PossibleAnswers = answers;
        }

        public override List<string> Answers => PossibleAnswers;

        public void AddPossibleAnswer(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
                throw new FieldException($"В {nameof(SelectOneQuestion)} ответ должен быть не null, или " +
            $"пустой строкой, или пробелом");

            PossibleAnswers.Add(answer);
        }

        public void RemovePossibleAnswer(string answer)
        {
            PossibleAnswers.Remove(answer);
        }

        public override List<string> SelectAnswer(params string[] answers)
        {
            if (!Required && answers.Length == 0)
                return answers.ToList();

            if (answers.Length != 1)
                throw new FieldException($"Поле {nameof(SelectOneQuestion)} должено иметь один ответ");

            if (!PossibleAnswers.Any(ans => ans == answers[0]))
                throw new FieldException($"В {nameof(SelectOneQuestion)} не существует такого ответа {answers}");

            return new List<string>() { answers[0] };
        }
    }
}
