using FluentValidation;
using MerosWebApi.Application.Common.ValidatorOptions;

namespace MerosWebApi.Application.Common
{
    public static class ValidatorExtensions
    {
        private static UserValidationOptions _userOptions;

        public static void InitValidatorExtensions(UserValidationOptions options)
        {
            _userOptions = options ?? throw new ArgumentNullException("UserValidationOptions is null!");
        }

        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilderOptions)
        {
            return ruleBuilderOptions
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(_userOptions.PasswordMinLength)
                .WithMessage($"Password must be at least {_userOptions.PasswordMinLength} characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
        }

        public static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilder<T, string> ruleBuilderOptions)
        {
            return ruleBuilderOptions.EmailAddress().WithMessage("Invalid email address.");
        }

        public static IRuleBuilderOptions<T, string> Fullname<T>(this IRuleBuilder<T, string> ruleBuilderOptions)
        {
            return ruleBuilderOptions
                .NotEmpty().WithMessage("Fullname can't be empty.")
                .MinimumLength(_userOptions.FullnameMinLength)
                .WithMessage($"Fullname must be at least {_userOptions.FullnameMinLength} characters long.")
                .MaximumLength(_userOptions.FullnameMaxLength)
                .WithMessage($"Fullname can't be longer than {_userOptions.FullnameMaxLength} characters long.");
        }
    }
}
