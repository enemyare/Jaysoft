using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

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
