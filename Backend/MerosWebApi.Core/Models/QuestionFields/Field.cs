using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models
{
    public abstract class Field
    {
        public Field(string text, string type, bool required)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new FieldException("Вопрос обязательно должен содержать текст");

            if (string.IsNullOrWhiteSpace(type))
                throw new FieldException("Тип вопроса должен содержать текст");

            Text = text;
            Type = type;
            Required = required;
        }

        public string Text { get; protected set; }

        public string Type { get; protected set; }

        public bool Required { get; protected set; }

        protected List<string> PossibleAnswers { get; set; }

        public abstract List<string> SelectAnswer(params string[] answers);

        public abstract List<string> Answers { get; }
    }
}
