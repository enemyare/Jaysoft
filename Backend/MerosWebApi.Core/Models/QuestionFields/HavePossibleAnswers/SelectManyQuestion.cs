using MerosWebApi.Core.Models.Exceptions;
using MerosWebApi.Core.Models.Questions;

namespace MerosWebApi.Core.Models.QuestionFields.HavePossibleAnswers
{
    internal class SelectManyQuestion : Field, IHavePossibleAnswers
    {
        public SelectManyQuestion(string questionText, bool required, List<string> answers)
            : base(questionText, "checkbox", required)
        {
            if (answers == null || answers.Count < 1)
                throw new FieldException($"Поле {Type} должно иметь как минимум " +
                                          $"один вариант ответа");

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
            if (answers.Length == 0 && !Required)
                return answers.ToList();

            if (answers.Length == 0)
                throw new FieldException($"Поле {Type} должено иметь минимум" +
                                         $" один ответа");

            foreach (var answer in answers)
            {
                if (!answers.Any(ans => ans == answer))
                    throw new FieldException($"В {Type} не существует такого ответа {answer}");
            }


            return answers.ToList();
        }
    }
}
