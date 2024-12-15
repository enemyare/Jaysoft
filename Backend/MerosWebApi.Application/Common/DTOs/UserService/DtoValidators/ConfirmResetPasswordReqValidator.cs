using FluentValidation;
using MerosWebApi.Application.Common.DTOs.UserService.ReqDtos;

namespace MerosWebApi.Application.Common.DTOs.UserService.DtoValidators
{
    public class ConfirmResetPasswordReqValidator : AbstractValidator<ConfirmResetPasswordQuery>
    {
        public ConfirmResetPasswordReqValidator()
        {
            RuleFor(c => c.Code).NotEmpty().WithMessage("Code is required");
            RuleFor(c => c.Email).Email();
        }
    }
}
