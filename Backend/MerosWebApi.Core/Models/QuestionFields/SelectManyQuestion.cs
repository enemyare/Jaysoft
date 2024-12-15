using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.Questions
{
    internal class SelectManyQuestion : Field, IHavePossibleAnswers
    {
        public SelectManyQuestion(string questionText, bool required, List<string> answers)
            : base(questionText, nameof(SelectManyQuestion), required)
        {
            if (answers == null || answers.Count < 1)
                throw new FieldException($"Поле {nameof(SelectManyQuestion)} должно иметь как минимум " +
                                          $"один вариант ответа");

            PossibleAnswers = answers;
        }

        public override List<string> Answers => PossibleAnswers;

        public void AddPossibleAnswer(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
                throw new FieldException($"В {nameof(SelectManyQuestion)} ответ должен быть не null, или " +
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
                throw new FieldException($"Поле {nameof(SelectManyQuestion)} должено иметь минимум" +
                                         $" один ответа");

            foreach (var answer in answers)
            {
                if (!answers.Any(ans => ans == answer))
                    throw new FieldException($"В {nameof(SelectManyQuestion)} не существует такого ответа {answer}");
            }


            return answers.ToList();
        }
    }
}
