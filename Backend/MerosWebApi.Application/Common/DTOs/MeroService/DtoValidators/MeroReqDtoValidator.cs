using FluentValidation;

namespace MerosWebApi.Application.Common.DTOs.MeroService.DtoValidators
{
    public class MeroReqDtoValidator : AbstractValidator<MeroReqDto>
    {
        public MeroReqDtoValidator()
        {
            RuleFor(a => a.MeetName)
                .Length(3, 100).WithMessage("Название мероприятя дожно содержать миниму 3 символа, максимум 100");
            RuleFor(a => a.Description)
                .MaximumLength(2000).WithMessage("Описание мероприятия должно содержать не больше 2000 символов");
            RuleFor(a => a.CreatorEmail)
                .Email();

            RuleFor(a => a.Periods)
                .NotEmpty().WithMessage("Список периодов записи не должен быть пустым")
                .ForEach(period =>
                    period.SetValidator(new TimePeriodReqDtoValidator()));

            RuleFor(mero => mero.Fields)
                .NotNull().WithMessage("Список периодов не может быть равен null")
                .ForEach(f => f.SetValidator(new FieldReqDtoValidator()));
        }
    }
}
