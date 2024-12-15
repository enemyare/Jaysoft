using FluentValidation;
using MongoDB.Bson;

namespace MerosWebApi.Application.Common.DTOs.MeroService.DtoValidators
{
    public class PhormAnswerReqDtoValidator : AbstractValidator<PhormAnswerReqDto>
    {
        public PhormAnswerReqDtoValidator()
        {
            RuleFor(phorm => phorm.MeroId)
                .Must(id => ObjectId.TryParse(id, out var objectId))
                .WithMessage("MeroId должен быть ObjectId");

            RuleFor(phorm => phorm.Answers)
                .ForEach(answer => answer.SetValidator(new AnswerReqDtoValidator()));

            RuleFor(phorm => phorm.TimePeriodId)
                .Must(id => ObjectId.TryParse(id, out var objectId))
                .WithMessage("TimePeriodId должен быть ObjectId");
        }
    }
}
