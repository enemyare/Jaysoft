using FluentValidation;

using MerosWebApi.Core.Models.QuestionFields;

namespace MerosWebApi.Application.Common.DTOs.MeroService.DtoValidators
{
    public class FieldReqDtoValidator : AbstractValidator<FieldReqDto>
    {
        public FieldReqDtoValidator()
        {
            RuleFor(field => field.Title)
                .NotEmpty().WithMessage("Поле вопроса должно иметь содержимое");

            RuleFor(field => field.Type)
                .Must(type => FieldFactoryMethod.FieldTypes.Contains(type))
                .WithMessage(type => $"Некорректный тип поля: {type.Type}");
        }
    }
}
