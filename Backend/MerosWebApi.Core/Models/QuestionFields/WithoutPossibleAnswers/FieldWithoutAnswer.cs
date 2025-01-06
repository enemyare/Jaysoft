using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.QuestionFields.WithoutPossibleAnswers
{
    public class FieldWithoutAnswer : Field
    {
        public FieldWithoutAnswer(string title)
            : base(title, "labelOnly")
        {
        }

        public override string SelectAnswer(string answer)
        {
            if (!string.IsNullOrWhiteSpace(answer))
                throw new FieldException($"{Type} не должно присваивать ответ(ы) (должно быть null)");

            return null;
        }
    }
}
