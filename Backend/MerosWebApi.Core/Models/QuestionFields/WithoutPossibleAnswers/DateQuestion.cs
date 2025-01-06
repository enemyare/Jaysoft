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
        public DateQuestion(string title) : base(title, "date")
        {
        }

        public override string SelectAnswer(string answer)
        {
            var oneAnswer = base.SelectAnswer(answer);

            if (!DateOnly.TryParseExact(oneAnswer, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var date))
            {
                throw new FieldException($"Поле '{Title}' должно иметь ответ представляющий дату в формате dd.MM.yyyy");
            }

            return oneAnswer;
        }
    }
}
