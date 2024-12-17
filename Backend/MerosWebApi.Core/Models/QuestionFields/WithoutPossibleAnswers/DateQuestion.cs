using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.QuestionFields.WithoutPossibleAnswers
{
    public class DateQuestion : WithoutPossibleAnswerQuestion
    {
        public DateQuestion(string label, bool required, List<string> answers) : base(label, "date", required, answers)
        {
        }

        public override List<string> SelectAnswer(params string[] answers)
        {
            var oneAnswer = base.SelectAnswer(answers);

            if (!DateOnly.TryParseExact(oneAnswer[0], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                throw new FieldException($"Поле \"{Type}\" должно иметь ответ представляющий тип в формате dd.MM.yyyy");
            }

            return oneAnswer;
        }
    }
}
