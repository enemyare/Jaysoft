using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MerosWebApi.Application.Common.DTOs.CommonDtos.CommonDtoValidators
{
    public class MustBeNotNegative : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int intValue)
            {
                if (intValue >= 0)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("The passed value must be not negative");
        }
    }
}
