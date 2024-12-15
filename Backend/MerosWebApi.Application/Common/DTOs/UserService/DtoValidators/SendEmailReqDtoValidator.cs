using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MerosWebApi.Application.Common.DTOs.UserService.ReqDtos;

namespace MerosWebApi.Application.Common.DTOs.UserService.DtoValidators
{
    public class SendEmailReqDtoValidator : AbstractValidator<SendEmailReqDto>
    {
        public SendEmailReqDtoValidator()
        {
            RuleFor(u => u.Email).Email();
        }
    }
}
