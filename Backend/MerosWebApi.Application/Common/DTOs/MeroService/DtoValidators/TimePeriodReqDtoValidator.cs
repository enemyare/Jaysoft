using FluentValidation;

namespace MerosWebApi.Application.Common.DTOs.MeroService.DtoValidators
{
    public class TimePeriodReqDtoValidator : AbstractValidator<TimePeriodsReqDto>
    {
        public TimePeriodReqDtoValidator()
        {
            RuleFor(period => period.TotalPlaces)
                .NotNull().WithMessage("Periods must not be null.")
                .Must(p => p > 0).WithMessage("Число мест на период мероприятия должно быть больше нуля");

            RuleFor(period => period.StartTime)
                .GreaterThan(DateTime.Now)
                .WithMessage("Дата начала проведения мероприятия должна быть позже чем настоящее время");

            RuleFor(period => period.EndTime)
                .GreaterThan(period => period.StartTime)
                .WithMessage("Время конца мероприятия должно быть позже чем время начала мероприятия");
        }
    }
}
