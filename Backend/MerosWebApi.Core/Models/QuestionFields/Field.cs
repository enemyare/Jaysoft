using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models
{
    public abstract class Field
    {
        public Field(string title, string type)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new FieldException("Вопрос обязательно должен содержать текст");

            if (string.IsNullOrWhiteSpace(type))
                throw new FieldException("Тип вопроса должен содержать текст");

            Title = title;
            Type = type;
        }

        public string Title { get; protected set; }

        public string Type { get; protected set; }

        public abstract string SelectAnswer(string answer);
    }
}
