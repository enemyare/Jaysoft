using MerosWebApi.Core.Models.Exceptions;
using MerosWebApi.Core.Models.PhormAnswer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.QuestionFields.WithoutPossibleAnswers
{
    public class WithoutPossibleAnswerQuestion : Field
    {
        public WithoutPossibleAnswerQuestion(string title, string type) : base(title, type)
        {
        }

        public override string SelectAnswer(string answer)
        {
            if (string.IsNullOrWhiteSpace(answer))
                throw new FieldException($"Ответ должен быть не пустой строкой, null, или пробелом");

            return answer;
        }
    }
}
