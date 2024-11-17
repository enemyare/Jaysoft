using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Models.QuestionFields;
using MerosWebApi.Persistence.Entites;

namespace MerosWebApi.Persistence.Helpers
{
    public class FieldPropertyAssigner
        : IPropertyAssigner<Field, DatabaseField>,
            IPropertyAssigner<DatabaseField, Field>,
            IPropertyValuesAssigner<DatabaseField, Field>
    {
        public static DatabaseField MapFrom(Field source)
        {
            return new DatabaseField
            {
                Text = source.Text,
                PossibleAnswers = source.Answers,
                Required = source.Required,
                Type = source.Type,
            };
        }

        public static Field MapFrom(DatabaseField source)
        {
            return FieldFactoryMethod.CreateField(source.Text, source.Type, source.Required, source.PossibleAnswers);
        }

        public static void AssignPropertyValues(DatabaseField to, Field from)
        {
            to.Text = from.Text;
            to.Type = from.Type;
            to.Required = from.Required;
            to.PossibleAnswers = from.Answers;
        }
    }
}
