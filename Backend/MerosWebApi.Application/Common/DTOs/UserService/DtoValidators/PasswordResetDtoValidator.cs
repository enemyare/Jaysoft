using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MerosWebApi.Application.Common.DTOs.UserService.DtoValidators
{
    public class PasswordResetDtoValidator : AbstractValidator<PasswordResetDto>
    {
        public PasswordResetDtoValidator()
        {
            RuleFor(a => a.Email).Email();
        }
    }
}
