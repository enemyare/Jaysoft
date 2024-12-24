using FluentValidation;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Models.Questions;
using MongoDB.Driver;
using System.Reflection;
using MerosWebApi.Core.Models.QuestionFields;

namespace MerosWebApi.Application.Common.DTOs.MeroService.DtoValidators
{
    public class FieldReqDtoValidator : AbstractValidator<FieldReqDto>
    {
        public FieldReqDtoValidator()
        {
            RuleFor(field => field.Label)
                .NotEmpty().WithMessage("Поле вопроса должно иметь содержимое");

            RuleFor(field => field.Type)
                .Must(type => FieldFactoryMethod.FieldTypes.Contains(type))
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
            if (FieldFactoryMethod.FieldWithPossibleTypes.Contains(field.Type))
            {
                if (field.Answers.Count < 0)
                    return (false, $"Число заданых возможных ответов вопроса \"{field.Label}\" должно быть > 0");
                if (field.Answers.Any(a => string.IsNullOrWhiteSpace(a)))
                    return (false, $"Текст ответа на вопрос \"{field.Label}\" должен быть не пустой строкой");
            }
            else if (field.Answers != null && FieldFactoryMethod.FieldTypes.Contains(field.Type))
                return (false, $"Вопрос '{field.Label}' должен иметь значение возможных ответов = null");

            return (true, $"Возможные ответы для типа '{field.Type}' - валидны");
        }
    }
}
