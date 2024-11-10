using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace MerosWebApi.Application.Common.DTOs.UserService.DtoValidators
{
    public class UpdateReqDtoValidator : AbstractValidator<UpdateReqDto>
    {
        public UpdateReqDtoValidator()
        {
            RuleFor(u => u.Email).Email().When(u => u.Email != null);
            RuleFor(u => u.Password).Password().When(u => u.Password != null);
            RuleFor(u => u.Full_name).Fullname().When(u => u.Full_name != null);
        }
    }
}
