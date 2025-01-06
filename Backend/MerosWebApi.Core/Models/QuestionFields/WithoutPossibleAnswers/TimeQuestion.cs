using MerosWebApi.Core.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.QuestionFields.WithoutPossibleAnswers
{
    public class TimeQuestion : WithoutPossibleAnswerQuestion
    {
        public TimeQuestion(string title) : base(title, "time")
        {
        }

        public override string SelectAnswer(string answer)
        {
            var oneAnswer = base.SelectAnswer(answer);

            if (!TimeOnly.TryParse(oneAnswer, out var date))
            {
                throw new FieldException($"Поле '{Title}' должно иметь ответ представляющий тип time");
            }

            return oneAnswer;
        }
    }
}
