using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace MerosWebApi.Application.Common.DTOs.CommonDtos.CommonDtoValidators;

public class MustBeObjectIdAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string strValue)
            if (ObjectId.TryParse(strValue, out _))
                return ValidationResult.Success;

        return new ValidationResult("The provided Id is not a valid ObjectId.");
    }
}