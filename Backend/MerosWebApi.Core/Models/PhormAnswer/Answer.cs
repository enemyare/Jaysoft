using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Models.PhormAnswer
{
    public class Answer
    {
        public string QuestionText { get; }

        public List<string> QuestionAnswers { get; }

        public Answer(string text, List<string> answers)
        {
            QuestionText = text;
            QuestionAnswers = answers;
        }
    }
}
