using FluentValidation;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Models.Questions;
using MongoDB.Driver;
using System.Reflection;

namespace MerosWebApi.Application.Common.DTOs.MeroService.DtoValidators
{
    public class FieldReqDtoValidator : AbstractValidator<FieldReqDto>
    {
        private static HashSet<string> fieldTypes;

        private static HashSet<string> fieldWithPossibleAnswers;

        static FieldReqDtoValidator()
        {
            Type fieldBaseType = typeof(Field);

            var assemblyTypes = Assembly.GetAssembly(fieldBaseType).GetTypes();

            var fieldBaseSubclasses = assemblyTypes
                .Where(t => t.IsSubclassOf(fieldBaseType));

            var possAnswerSubclasses = assemblyTypes
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IHavePossibleAnswers).IsAssignableFrom(t));

            fieldTypes = fieldBaseSubclasses
                .Select(t => t.Name)
                .ToHashSet();

            fieldWithPossibleAnswers = possAnswerSubclasses
                .Select(t => t.Name)
                .ToHashSet();
        }

        public FieldReqDtoValidator()
        {
            RuleFor(field => field.Text)
                .NotEmpty().WithMessage("Поле вопроса должно иметь содержимое");

            RuleFor(field => field.Type)
                .Must(type => fieldTypes.Contains(type))
                .WithMessage(type => $"Некорректный тип поля: {type.Type}");

            (bool valid, string message) answerValidResult = (false, String.Empty);

            RuleFor(field => field)
                .Must((field) =>
                {
                    answerValidResult = IsValidateFieldAnswers(field);
                    return answerValidResult.valid;
                })
                .WithMessage(f => answerValidResult.message);
        }

        private (bool valid, string answer) IsValidateFieldAnswers(FieldReqDto field)
        {
            if (fieldWithPossibleAnswers.Contains(field.Type))
            {
                if (field.Answers.Count < 0)
                    return (false, $"Число заданых возможных ответов вопроса \"{field.Text}\" должно быть > 0");
                if (field.Answers.Any(a => string.IsNullOrWhiteSpace(a)))
                    return (false, $"Текст ответа на вопрос \"{field.Text}\" должен быть не пустой строкой");
            }
            else if (field.Answers != null && fieldTypes.Contains(field.Type))
                return (false, $"Вопрос '{field.Text}' должен иметь значение возможных ответов = null");

            return (true, $"Возможные ответы для типа '{field.Type}' - валидны");
        }
    }
}
