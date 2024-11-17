using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Exceptions;
using static System.Net.Mime.MediaTypeNames;

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
