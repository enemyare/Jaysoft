using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MerosWebApi.Application.Common;
using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Common.SecurityHelpers;
using MerosWebApi.Application.Interfaces;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Repository;
using Microsoft.Extensions.FileProviders;
using ZstdSharp.Unsafe;

namespace MerosWebApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        private readonly IPasswordHelper _passwordHelper;

        private readonly ITokenGenerator _tokenGenerator;

        private readonly IEmailSender _emailSender;

        private readonly AppSettings _appSettings;

        private readonly EmbeddedFileProvider _embedded;

        public UserService(IUserRepository repository, IPasswordHelper passwordHelper,
            IEmailSender emailSender, AppSettings appSettings, ITokenGenerator generator)
        {
            _repository = repository;
            _passwordHelper = passwordHelper;
            _emailSender = emailSender;
            _appSettings = appSettings;
            _embedded = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            _tokenGenerator = generator;
        }

        public async Task<AuthenticationResDto> AuthenticateAsync(AuthenticateReqDto dto)
        {
            var user = await _repository.GetUserByEmail(dto.Email);

            if (user == null)
                throw new AuthenticationException("The email or password is incorrect");

            if (user.LoginFailedAt != null)
            {
                var secondsPassed = DateTime.UtcNow.Subtract(
                    user.LoginFailedAt.GetValueOrDefault()).Seconds;

                var isMaxCountExceeded = user.LoginFailedCount >= _appSettings.MaxLoginFailedCount;
                var isWaitingTimePassed = secondsPassed > _appSettings.LoginFailedWaitingTime;

                if (isMaxCountExceeded && !isWaitingTimePassed)
                {
                    var secondsToWait = _appSettings.LoginFailedWaitingTime - secondsPassed;
                    
                    throw new TooManyFailedLoginAttemptsException(string.Format(
                        "You must wait for {0} seconds before you try to log in again.", secondsToWait));
                }
            }

            if (!_passwordHelper.VerifyHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            {
                user.LoginFailedCount += 1;
                user.LoginFailedAt = DateTime.Now;
                await _repository.UpdateUser(user);
                throw new AuthenticationException("The email or password is incorrect");
            }

            //Authentication successful
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            user.LoginFailedCount = 0;
            user.LoginFailedAt = null;
            user.LastLoginAt = DateTime.Now;
            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = DateTime.Now;
            user.TokenExpires = refreshToken.Expires;

            await _repository.UpdateUser(user);

            var responseDto = AuthenticationResDto.Map(user);

            responseDto.AccessToken = _tokenGenerator.GenerateAccessToken(user.Id.ToString());
            responseDto.RefreshToken = refreshToken.Token;

            return responseDto;
        }

        //Did
        public async Task<GetDetailsResDto> RegisterAsync(RegisterReqDto dto)
        {
            if (string.IsNullOrEmpty(dto.Password))
                throw new InvalidPasswordException("Password is required");

            var existingUser = await _repository.GetUserByEmail(dto.Email);

            if (existingUser?.Email == dto.Email)
                throw new EmailTakenException($"Email {dto.Email} us already taken.");

            var (passwordHash, passwordSalt) = _passwordHelper.CreateHash(dto.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Full_name = dto.Full_name,
                CreatedAt = DateTime.Now,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsActive = false
            };

            var emailSuccess = await ChangeEmailAsync(user, dto.Email);
            //Must confirm Email

            if (!emailSuccess)
                throw new EmailNotSentException("Sending of confirmation email failed.");

            await _repository.AddUser(user);

            return GetDetailsResDto.Map(user);
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {
            var user = await _repository.GetUserByRefreshToken(refreshToken);

            if (user == null)
                throw new EntityNotFoundException("User with such refreshToken didn't founded");

            if (user.TokenExpires < DateTime.Now)
                throw new TimeExpiredException("The refresh token has expired");

            return _tokenGenerator.GenerateAccessToken(user.Id.ToString());
        }

        public async Task<GetDetailsResDto> GetDetailsAsync(Guid id)
        {
            var user = await _repository.GetUserById(id);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            return GetDetailsResDto.Map(user);
        }

        public async Task<GetDetailsResDto> UpdateAsync(Guid id, Guid userId,
            UpdateReqDto dto)
        {
            if (userId != id)
                throw new ForbiddenException("Доступ запрещен");

            var user = await _repository.GetUserById(id);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            if (dto.Full_name != null && dto.Full_name != user.Full_name)
                user.Full_name = dto.Full_name;

            if (dto.Email != null)
            {
                var emailSuccess = await ChangeEmailAsync(user, dto.Email);

                if (!emailSuccess)
                    throw new EmailNotSentException("Sending of confirmation email failed");
            }

            if (dto.Password != null)
            {
                var (passwordHash, passwordSalt) = _passwordHelper.CreateHash(dto.Password);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            user.UpdatedAt = DateTime.Now;
            _repository.UpdateUser(user);

            return GetDetailsResDto.Map(user);
        }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            if (userId != id) throw new ForbiddenException("Доступ запрещен");

            return await _repository.DeleteUser(id);
        }

        public async Task ConfirmEmailAsync(string code)
        {
            var user = await _repository.GetUserByUnconfirmedCode(code);
            if (user == null)
                throw new EntityNotFoundException("Somethin went wrong... Please contact support");

            if (user.Email == null)
                user.IsActive = true;

            user.Email = user.UnconfirmedEmail;
            user.UnconfirmedEmail = null;
            user.UnconfirmedEmailCode = null;
            user.UnconfirmedEmailCount = 0;
            user.UnconfirmedEmailCreatedAt = null;

            await _repository.UpdateUser(user);
        }

        public async Task PasswordResetAsync(PasswordResetDto dto)
        {
            var user = await _repository.GetUserByEmail(dto.Email);
            if (user == null)
                throw new EntityNotFoundException("Something went wrong... Please contact support.");

            var secondsPassed = DateTime.Now.Subtract(user.ResetPasswordCreatedAt.GetValueOrDefault()).Seconds;

            var isMaxCountExceeded = user.ResetPasswordCount >= _appSettings.MaxResetPasswordCount;
            var isWaitingTimePassed = secondsPassed > _appSettings.ResetPasswordWaitingTime;

            if (isMaxCountExceeded && !isWaitingTimePassed)
            {
                var secondsToWait = _appSettings.ResetPasswordWaitingTime - secondsPassed;
                throw new TooManyResetPasswordAttemptsException(
                    $"You must wait for {secondsToWait} seconds before you try reset password again");
            }

            user.ResetPasswordCode = _passwordHelper.GenerateRandomString(20) + Guid.NewGuid();
            user.ResetPasswordCount += 1;
            user.ResetPasswordCreatedAt = DateTime.Now;

            var emailSuccess = await SentResetPasswordCode(user);
            if (!emailSuccess)
                throw new EmailNotSentException("Sending of email failed.");

            _repository.UpdateUser(user);
        }

        public async Task<ConfirmResetPswdDto> ConfirmResetPasswordAsync(string code,
            string email)
        {
            var user = await _repository.GetUserByResetCode(code, email);
            
            if(user == null || user.ResetPasswordCode == null)
                throw new EntityNotFoundException("Invalid code.");

            var secondsPassed = DateTime.Now.Subtract(user.ResetPasswordCreatedAt.GetValueOrDefault()).Seconds;
            if (secondsPassed > _appSettings.ResetPasswordValidTime)
                throw new AppException("This link has exprired... Please try to reset password again");

            user.ResetPasswordCode = null;
            user.ResetPasswordCount = 0;
            user.ResetPasswordCreatedAt = null;

            var newPassword = _passwordHelper.GenerateRandomString(8);

            (user.PasswordHash, user.PasswordSalt) = _passwordHelper.CreateHash(newPassword);

            _repository.UpdateUser(user);

            var dto = ConfirmResetPswdDto.CreateFromUser(user);
            dto.Password = newPassword;

            return dto;
        }

        #region Private helper methods

        private async Task<bool> SentResetPasswordCode(User user)
        {
            // Prepare email template.
            var parentDir = Directory.GetParent(Directory.GetCurrentDirectory());
            string relativePath = Path.Combine(parentDir.FullName,
                "MerosWebApi.Application/Common/EmailSender/EmailTemplates/Email_PasswordReset.html");

            await using var stream = File.OpenRead(relativePath);
            
            var encPasswordCode = HttpUtility.UrlEncode(user.ResetPasswordCode);
            var encEmail = HttpUtility.UrlEncode(user.Email);
            var emailBody = await new StreamReader(stream).ReadToEndAsync();
            emailBody = emailBody.Replace("{{APP_NAME}}", _appSettings.Name);
            emailBody = emailBody.Replace("{{PASSWORD_RESET_CONFIRM_URL}}",
                $"{_appSettings.ResetPasswordUrl}?code={encPasswordCode}&email={encEmail}");

            // Send an email.
            return await _emailSender.SendAsync(user.Email, "Reset your password", emailBody);
        }

        private async Task<bool> ChangeEmailAsync(User user, string newEmail)
        {
            if (newEmail == user.Email) return true;

            var secondsPassed = DateTime.Now.Subtract(
                user.UnconfirmedEmailCreatedAt.GetValueOrDefault()).Seconds;

            var isMaxCountExceeded = user.UnconfirmedEmailCount >= _appSettings.MaxUnconfirmedEmailCount;
            var isWaitingTimePassed = secondsPassed > _appSettings.UnconfirmedEmailWaitingTime;

            if (isMaxCountExceeded && !isWaitingTimePassed)
            {
                var secondsToWait = _appSettings.UnconfirmedEmailWaitingTime - secondsPassed;

                throw new TooManyChangeEmailAttemptsException(
                    string.Format("You must wait for {0} seconds before you try to change email again."),
                    secondsToWait);
            }

            user.UnconfirmedEmail = newEmail;
            user.UnconfirmedEmailCode = _passwordHelper.GenerateRandomString(30) + Guid.NewGuid();
            user.UnconfirmedEmailCount += 1;
            user.UnconfirmedEmailCreatedAt = DateTime.UtcNow;

            // Prepare email template.
            var parentDir = Directory.GetParent(Directory.GetCurrentDirectory());
            string relativePath = Path.Combine(parentDir.FullName,
                "MerosWebApi.Application/Common/EmailSender/EmailTemplates/Email_ConfirmEmail.html");

            // Prepare email template.
            await using var stream = File.OpenRead(relativePath);

            var emailBody = await new StreamReader(stream).ReadToEndAsync();
            emailBody = emailBody.Replace("{{APP_NAME}}", _appSettings.Name);
            emailBody = emailBody.Replace("{{EMAIL_CONFIRM_URL}}",
                $"{_appSettings.ConfirmEmailUrl}?code={user.UnconfirmedEmailCode}");

            // Send an email.
            return await _emailSender.SendAsync(newEmail, "Confirm your email", emailBody);
        }

        #endregion
    }
}
