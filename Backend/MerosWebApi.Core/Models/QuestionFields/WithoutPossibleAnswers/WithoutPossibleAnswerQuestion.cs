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
        public WithoutPossibleAnswerQuestion(string title, string type, List<string> answers) : base(title, type)
        {
            if (answers != null)
                throw new FieldException($"{Type} не должно иметь варианты ответов");
        }

        public override List<string> SelectAnswer(params string[] answers)
        {
            if (answers.Length != 1)
                throw new FieldException($"Поле {Type} должено иметь один ответ");

            return new List<string>() { answers[0] };
        }

        public override List<string> Answers => null;
    }
}
