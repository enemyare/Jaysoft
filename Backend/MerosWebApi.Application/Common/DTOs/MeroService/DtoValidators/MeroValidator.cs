using FluentValidation;
using MerosWebApi.Application.Common.DTOs.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Application.Common.DTOs.MeroService.DtoValidators
{
    public class MeroValidator : AbstractValidator<MeroReqDto>
    {
        public MeroValidator()
        {
            RuleFor(a => a.MeetName).Length(3, 100);
            RuleFor(a => a.Description).Length(0, 100);
            RuleFor(a => a.CreatorEmail).Email();
        }
    }
}
