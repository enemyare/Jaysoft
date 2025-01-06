using MerosWebApi.Core.Models;
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
                Title = source.Title,
                PossibleAnswers = source.Answers,
                Type = source.Type,
            };
        }

        public static Field MapFrom(DatabaseField source)
        {
            return FieldFactoryMethod.CreateField(source.Title, source.Type, source.PossibleAnswers);
        }

        public static void AssignPropertyValues(DatabaseField to, Field from)
        {
            to.Title = from.Title;
            to.Type = from.Type;
            to.PossibleAnswers = from.Answers;
        }
    }
}
