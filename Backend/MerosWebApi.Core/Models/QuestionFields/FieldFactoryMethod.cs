using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models.Exceptions;

namespace MerosWebApi.Core.Models.QuestionFields
{
    public static class FieldFactoryMethod
    {
        private static readonly Dictionary<string, Func<string, bool, List<string>, Field>> constructorInfos;

        static FieldFactoryMethod()
        {
            constructorInfos = new Dictionary<string, Func<string, bool, List<string>, Field>>();

            Type baseType = typeof(Field);
            var derivedTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(baseType));

            foreach (var type in derivedTypes)
            {
                var className = type.Name;

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

                constructorInfos.Add(className, lambda.Compile());
            }
        }

        public static Field CreateField(string text, string type, bool required, List<string> answers)
        {
            if (!constructorInfos.TryGetValue(type, out var constructor))
                throw new FieldTypeException($"Передан несуществующий тип для создания поля  - {type}");

            return constructor(text, required, answers);
        }
    }
}
