using System.Linq.Expressions;
using System.Reflection;
using MerosWebApi.Core.Models.Exceptions;
using MerosWebApi.Core.Models.QuestionFields.WithoutPossibleAnswers;

namespace MerosWebApi.Core.Models.QuestionFields
{
    public static class FieldFactoryMethod
    {
        private static readonly Dictionary<string, Func<string, Field>> constructorInfos = new();

        public static readonly HashSet<string> FieldTypes = new();

        static FieldFactoryMethod()
        {
            Type baseType = typeof(Field);
            var derivedTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(baseType) && t != typeof(WithoutPossibleAnswerQuestion));

            foreach (var type in derivedTypes)
            {
                var constructor = type.GetConstructors()[0];

                var titleParam = Expression.Parameter(typeof(string), "title");

                var args = new List<Expression>
                {
                    titleParam
                };

                var newExpression = Expression.New(constructor, args);
                var lambda = Expression.Lambda<Func<string, Field>>
                    (newExpression, titleParam);

                var fieldTypeString = MatchFieldByType(type);

                FieldTypes.Add(fieldTypeString);

                constructorInfos.Add(fieldTypeString, lambda.Compile());
            }
        }

        public static Field CreateField(string title, string type)
        {
            if (!constructorInfos.TryGetValue(type, out var constructor))
                throw new FieldTypeException($"Передан несуществующий тип для создания поля  - {type}");

            return constructor(title);
        }

        public static string MatchFieldByType(Type type)
        {
            return type switch
            {
                var t when t == typeof(TextQuestion) => "text",
                var t when t == typeof(TimeQuestion) => "time",
                var t when t == typeof(DateQuestion) => "date",
                var t when t == typeof(FieldWithoutAnswer) => "labelOnly",
                _ => throw new FieldTypeException($"Передан несуществующий тип для создания поля  - {type}")
            };
        }
    }
}
