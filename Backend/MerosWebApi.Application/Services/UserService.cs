﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MerosWebApi.Application.Common;
using MerosWebApi.Application.Common.DTOs.MeroService;
using MerosWebApi.Application.Common.DTOs.UserService;
using MerosWebApi.Application.Common.Exceptions;
using MerosWebApi.Application.Common.SecurityHelpers;
using MerosWebApi.Application.Interfaces;
using MerosWebApi.Core.Models.User;
using MerosWebApi.Core.Repository;
using Microsoft.Extensions.FileProviders;
using MongoDB.Bson;
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

        const int AUTH_CODE_LENGTH = 7;

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

        public async Task<AuthenticationResDto> AuthenticateAsync(string authCode)
        {
            var user = await _repository.GetUserByVerificationCode(authCode);

            if (user == null)
                throw new AuthenticationException("Пользователем с таким кодом авторизации не найден");

            if (user.LoginFailedAt != null)
            {
                var loginFailedPassed = DateTime.Now.Subtract(
                    user.LoginFailedAt.GetValueOrDefault()).Seconds;

                var verifCodePassed = DateTime.Now.Subtract(
                    user.VerificationCodeCreatedAt.GetValueOrDefault()).Minutes;

                var isMaxCountExceeded = user.LoginFailedCount >= _appSettings.MaxLoginFailedCount;
                var isWaitingTimePassed = loginFailedPassed > _appSettings.LoginFailedWaitingTime;
                var isVerifCodeExpires = verifCodePassed > _appSettings.VerificationCodeExpiresMinutes;

                if (isVerifCodeExpires)
                {
                    throw new TimeExpiredException("Срок дейстия кода авторизации истек, пожалуйста получите новый");
                }
                if (isMaxCountExceeded && !isWaitingTimePassed)
                {
                    var secondsToWait = _appSettings.LoginFailedWaitingTime - loginFailedPassed;
                    
                    throw new TooManyFailedLoginAttemptsException(string.Format(
                        "You must wait for {0} seconds before you try to log in again.", secondsToWait));
                }
            }

            if (authCode != user.VerificationCode)
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
            user.VerificationCode = null;
            user.VerificationCodeCount = 0;
            user.VerificationCodeCreatedAt = null;
            

            await _repository.UpdateUser(user);

            var responseDto = AuthenticationResDto.Map(user);

            responseDto.AccessToken = _tokenGenerator.GenerateAccessToken(user.Id.ToString());
            responseDto.RefreshToken = refreshToken.Token;

            return responseDto;
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

        public async Task<GetDetailsResDto> GetDetailsAsync(string id)
        {
            var user = await _repository.GetUserById(id);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            return GetDetailsResDto.Map(user);
        }

        public async Task<GetDetailsResDto> UpdateAsync(string id, string userId,
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

            user.UpdatedAt = DateTime.Now;
            _repository.UpdateUser(user);

            return GetDetailsResDto.Map(user);
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            if (userId != id) throw new ForbiddenException("Доступ запрещен");

            return await _repository.DeleteUser(id);
        }

        public async Task<UserStatisticResDto> GetUserStatisticAsync(string userId)
        {
            var user = await _repository.GetUserById(userId);
            if (user == null)
                throw new EntityNotFoundException("User not found");

            var querryResult = await _repository.GetUserStatisticById(userId);
            return new UserStatisticResDto
            {
                CreatedMerosCount = querryResult.CreatedMerosCount,
                ParticipantsCount = querryResult.ParticipantsCount,
                UserRegistredMerosCount = querryResult.UserRegistredMerosCount,
            };
        }

        public async Task<bool> SendUserUniqueInviteCode(string email)
        {
            var user = await _repository.GetUserByEmail(email);
            var userExists = true;

            if (user == null)
            {
                userExists = false;
                user = new User();
                user.Email = email;
            }

            var secondsPassed = DateTime.Now.Subtract(
                user.VerificationCodeCreatedAt.GetValueOrDefault()).Seconds;

            var isMaxCountExceeded = user.VerificationCodeCount >= _appSettings.MaxVerificationCodeCount;
            var isWaitingTimePassed = secondsPassed > _appSettings.VerificationCodeWaitingTime;

            if (isMaxCountExceeded && !isWaitingTimePassed)
            {
                var secondsToWait = _appSettings.VerificationCodeWaitingTime - secondsPassed;

                throw new TooManyChangeEmailAttemptsException(
                    string.Format("You must wait for {0} seconds before you try to change email again.",
                    secondsToWait));
            }

            user.VerificationCode = await CreateUniqueInviteCode();
            user.VerificationCodeCount += 1;
            user.VerificationCodeCreatedAt = DateTime.Now;

            // Prepare email template.
            string relativePath = Path.Combine("Resources",
                "EmailTemplates/Email_GetInviteCode.html");

            // Prepare email template.
            await using var stream = File.OpenRead(relativePath);

            var emailBody = await new StreamReader(stream).ReadToEndAsync();
            emailBody = emailBody.Replace("{{APP_NAME}}", _appSettings.Name);
            emailBody = emailBody.Replace("{{UNIQUE_LOGIN_CODE}}",
                user.VerificationCode);

            // Send an email.
            var sendingResult = await _emailSender.SendAsync(user.Email, "Подтверждение кода входа", emailBody);
            if (sendingResult)
            {
                if (userExists)
                    await _repository.UpdateUser(user);
                else
                {
                    await _repository.AddUser(user);
                }
            }

            return sendingResult;
        }

        public async Task ConfirmEmailAsync(string code)
        {
            var user = await _repository.GetUserByUnconfirmedCode(code);
            if (user == null)
                throw new EntityNotFoundException("Somethin went wrong... Please contact support");

            user.Email = user.UnconfirmedEmail;
            user.UnconfirmedEmail = null;
            user.UnconfirmedEmailCode = null;
            user.UnconfirmedEmailCount = 0;
            user.UnconfirmedEmailCreatedAt = null;

            await _repository.UpdateUser(user);
        }

        #region Private helper methods
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
            string relativePath = Path.Combine("Resources",
                "EmailTemplates/Email_ConfirmEmail.html");

            // Prepare email template.
            await using var stream = File.OpenRead(relativePath);

            var emailBody = await new StreamReader(stream).ReadToEndAsync();
            emailBody = emailBody.Replace("{{APP_NAME}}", _appSettings.Name);
            emailBody = emailBody.Replace("{{EMAIL_CONFIRM_URL}}",
                $"{_appSettings.ConfirmEmailUrl}?code={user.UnconfirmedEmailCode}");

            // Send an email.
            return await _emailSender.SendAsync(newEmail, "Confirm your email", emailBody);
        }

        private async Task<string> CreateUniqueInviteCode()
        {
            var inviteCode = RandomStringGenerator.GenerateRandomString(AUTH_CODE_LENGTH);

            while (await _repository.GetUserByVerificationCode(inviteCode) != null)
            {
                inviteCode = RandomStringGenerator.GenerateRandomString(AUTH_CODE_LENGTH);
            }

            return inviteCode;
        }

        #endregion
    }
}
