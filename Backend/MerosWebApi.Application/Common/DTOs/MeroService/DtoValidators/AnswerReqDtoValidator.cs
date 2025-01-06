using FluentValidation;
using MerosWebApi.Application.Common.DTOs.MeroService.ReqDtos;

namespace MerosWebApi.Application.Common.DTOs.MeroService.DtoValidators
{
    public class AnswerReqDtoValidator : AbstractValidator<AnswerReqDto>
    {
        public AnswerReqDtoValidator()
        {
            RuleFor(answer => answer.QuestionTitle)
                .NotEmpty().WithMessage("Поле вопроса должно иметь содержимое");

            RuleFor(answer => answer.QuestionAnswer)
                .Must((answer) => ValidateQuestionAnswer(answer))
                .WithMessage("Ответ вопроса должен быть либо null, либо содержать список не пустых строк");
        }

        private bool ValidateQuestionAnswer(string answer)
        {
            //if (string.IsNullOrWhiteSpace(answer))
            //    return false;

            return true;
        }
    }
}
