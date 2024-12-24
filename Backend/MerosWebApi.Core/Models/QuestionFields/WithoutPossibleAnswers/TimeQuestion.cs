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
        public TimeQuestion(string label,  bool required, List<string> answers) : base(label, "time", required, answers)
        {
        }

        public override List<string> SelectAnswer(params string[] answers)
        {
            var oneAnswer = base.SelectAnswer(answers);

            if (!TimeOnly.TryParse(oneAnswer[0], out var date))
            {
                throw new FieldException($"Поле \"{Type}\" должно иметь ответ представляющий тип time");
            }

            return oneAnswer;
        }
    }
}
