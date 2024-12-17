using MerosWebApi.Core.Models.Exceptions;
using MerosWebApi.Core.Models.Questions;

namespace MerosWebApi.Core.Models.QuestionFields.HavePossibleAnswers
{
    public class SelectOneQuestion : Field, IHavePossibleAnswers
    {
        public SelectOneQuestion(string questionText, bool required, List<string> answers)
            : base(questionText, "radiobutton", required)
        {
            if (answers == null || answers.Count < 1)
                throw new FieldException($"Поле {Type} должно иметь как " +
                                         $"минимум один вариант ответа");

            PossibleAnswers = answers;
        }

        public override List<string> Answers => PossibleAnswers;

        public void AddPossibleAnswer(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
                throw new FieldException($"В {Type} ответ должен быть не null, или " +
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
                throw new FieldException($"Поле {Type} должено иметь один ответ");

            if (!PossibleAnswers.Any(ans => ans == answers[0]))
                throw new FieldException($"В {Type} не существует такого ответа {answers}");

            return new List<string>() { answers[0] };
        }
    }
}
