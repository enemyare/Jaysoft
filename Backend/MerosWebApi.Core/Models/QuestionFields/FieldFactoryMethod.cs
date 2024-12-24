using MerosWebApi.Core.Models.Exceptions;
using System.Linq.Expressions;
using System.Reflection;
using MerosWebApi.Core.Models.QuestionFields.HavePossibleAnswers;
using MerosWebApi.Core.Models.QuestionFields.WithoutPossibleAnswers;
using MerosWebApi.Core.Models.Questions;

namespace MerosWebApi.Core.Models.QuestionFields
{
    public static class FieldFactoryMethod
    {
        private static readonly Dictionary<string, Func<string, bool, List<string>, Field>> constructorInfos = new();

        public static readonly HashSet<string> FieldTypes = new();

        public static readonly HashSet<string> FieldWithPossibleTypes = new();

        static FieldFactoryMethod()
        {
            Type baseType = typeof(Field);
            var derivedTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(baseType) && t != typeof(WithoutPossibleAnswerQuestion));

            foreach (var type in derivedTypes)
            {
                var constructor = type.GetConstructors()[0];
                var parameters = constructor.GetParameters();

                var textParam = Expression.Parameter(typeof(string), "text");
                var requiredParam = Expression.Parameter(typeof(bool), "required");
                var answersParam = Expression.Parameter(typeof(List<string>), "answers");

                var args = new List<Expression>
                {
                    textParam,
                    requiredParam,
                    answersParam
                };

                var newExpression = Expression.New(constructor, args);
                var lambda = Expression.Lambda<Func<string, bool, List<string>, Field>>
                    (newExpression, textParam, requiredParam, answersParam);

                var fieldTypeString = MatchFieldByType(type);

                FieldTypes.Add(fieldTypeString);

                if (typeof(IHavePossibleAnswers).IsAssignableFrom(type))
                {
                    FieldWithPossibleTypes.Add(fieldTypeString);
                }

                constructorInfos.Add(fieldTypeString, lambda.Compile());
            }
        }

        public static Field CreateField(string text, string type, bool required, List<string> answers)
        {
            if (!constructorInfos.TryGetValue(type, out var constructor))
                throw new FieldTypeException($"Передан несуществующий тип для создания поля  - {type}");

            return constructor(text, required, answers);
        }

        public static string MatchFieldByType(Type type)
        {
            return type switch
            {
                var t when t == typeof(SelectOneQuestion) => "radiobutton",
                var t when t == typeof(SelectManyQuestion) => "checkbox",
                var t when t == typeof(TextQuestion) => "text",
                var t when t == typeof(TimeQuestion) => "time",
                var t when t == typeof(DateQuestion) => "date",
                var t when t == typeof(FieldWithoutAnswer) => "labelOnly",
                _ => throw new FieldTypeException($"Передан несуществующий тип для создания поля  - {type}")
            };
        }
    }
}
