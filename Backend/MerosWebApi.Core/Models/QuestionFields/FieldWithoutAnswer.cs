using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.Questions
{
    public class FieldWithoutAnswer : Field
    {
        public FieldWithoutAnswer(string text, bool required, List<string> answers)
            : base(text, nameof(FieldWithoutAnswer), required)
        {
            if (answers != null)
                throw new FieldException($"{nameof(FieldWithoutAnswer)} не должно иметь варианты ответов");
        }

        public override List<string> SelectAnswer(params string[] answers)
        {
            if (answers.Length > 0)
                throw new FieldException($"{nameof(FieldWithoutAnswer)} не должно присваивать ответ(ы)");

            return null;
        }

        public override List<string> Answers => null;
    }
}
