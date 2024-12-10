﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Application.Common.EmailSender;
using MerosWebApi.Application.Interfaces;
using MerosWebApi.Application.Common;
using MerosWebApi.Application.Services;
using MerosWebApi.Application.Common.SecurityHelpers;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using FluentValidation;
using FluentValidation.AspNetCore;
using MerosWebApi.Application.Common.DTOs;
using MerosWebApi.Application.Common.ValidatorOptions;
using MerosWebApi.Core.Repository;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace MerosWebApi.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDevEmailConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            var devConfiguration = configuration.GetSection("DevelopmentEmailConfiguration");

            var hostAddress = devConfiguration["Host"];
            int.TryParse(devConfiguration["Port"], out var hostPort);
            var emailAddress = devConfiguration["UserName"];
            var emailPassword = devConfiguration["Password"];

            if (string.IsNullOrWhiteSpace(hostAddress) || hostPort == default(int) ||
                string.IsNullOrWhiteSpace(emailAddress) || string.IsNullOrWhiteSpace(emailPassword))
            {
                throw new ArgumentException($"Не правильно сконфигурирована" +
                    $"секция '{nameof(devConfiguration.Value)}' в 'appsettings.json'");
            }


            var emailConfiguration = new DevelopmentConfiguration(emailAddress, emailPassword, hostAddress,
                hostPort);

            services.AddSingleton<IEmailConfiguration>(emailConfiguration);

            return services;
        }

        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");

            var name = appSettingsSection["Name"];
            int.TryParse(appSettingsSection["MaxLoginFailedCount"], out var maxLoginFailed);
            int.TryParse(appSettingsSection["LoginFailedWaitingTime"], out var loginFailedWait);
            int.TryParse(appSettingsSection["MaxVerificationCodeCount"], out var maxVerCount);
            int.TryParse(appSettingsSection["VerificationCodeWaitingTime"], out var verificationWait);
            int.TryParse(appSettingsSection["MaxUnconfirmedEmailCount"], out var maxUnconfEmail);
            int.TryParse(appSettingsSection["UnconfirmedEmailWaitingTime"], out var unconfEmailWait);
            int.TryParse(appSettingsSection["VerificationCodeExpiresMinutes"], out var verifCodeExp);
            var confEmailUrl = appSettingsSection["ConfirmEmailUrl"];
            int.TryParse(appSettingsSection["MaxResetPasswordCount"], out var maxResetPasswd);
            int.TryParse(appSettingsSection["ResetPasswordWaitingTime"], out var resetPasswdWait);
            int.TryParse(appSettingsSection["ResetPasswordValidTime"], out var resetPasswdValid);
            var resetPasswdUrl = appSettingsSection["ResetPasswordUrl"];

            //В будущем добавить логику проверки полученных значений
            //И/или переделать конфигурирование AppSettings

            var appSettings = new AppSettings
            {
                Name = name,
                MaxLoginFailedCount = maxLoginFailed,
                LoginFailedWaitingTime = loginFailedWait,
                MaxUnconfirmedEmailCount = maxUnconfEmail,
                UnconfirmedEmailWaitingTime = unconfEmailWait,
                MaxVerificationCodeCount = maxVerCount,
                VerificationCodeWaitingTime = verificationWait,
                VerificationCodeExpiresMinutes = verifCodeExp,
                ConfirmEmailUrl = confEmailUrl,
                MaxResetPasswordCount = maxResetPasswd,
                ResetPasswordWaitingTime = resetPasswdWait,
                ResetPasswordValidTime = resetPasswdValid,
                ResetPasswordUrl = resetPasswdUrl
            };

            services.AddSingleton<AppSettings>(appSettings);

            return services;
        }

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IAuthHelper, AuthHelper>();

            services.AddScoped<IMeroService, MeroService>();

            return services;
        }

        public static IServiceCollection AddSecurityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));
            services.AddScoped<IPasswordHelper, PasswordHelper>();
            services.AddScoped<ITokenGenerator, TokensGenerator>();

            var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

            services
                .AddAuthentication(configuration =>
                {
                    configuration.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    configuration.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(configuration =>
                {
                    configuration.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var repository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                            var userId = context.Principal?.Identity?.Name;
                            if (userId == null)
                            {
                                context.Fail("Unauthorized");
                                return Task.CompletedTask;
                            }

                            var user = repository.GetUserById(userId);

                            if (user == null)
                            {
                                context.Fail("Unauthorized");
                                return Task.CompletedTask;
                            }

                            return Task.CompletedTask;
                        },
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Headers.Authorization;

                            return Task.CompletedTask;
                        }
                    };
                    configuration.RequireHttpsMetadata = false;
                    configuration.SaveToken = true;
                    configuration.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddControllersAndFluentValidatation(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<UserValidationOptions>(configuration.GetSection(nameof(UserValidationOptions)));

            var userValidOptions = configuration.GetSection(nameof(UserValidationOptions)).Get<UserValidationOptions>();

            ValidatorExtensions.InitValidatorExtensions(userValidOptions);

            // Регистрация валидаторов из текущей сборки
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var errors = context.ModelState
                            .Where(e => e.Value.Errors.Count > 0)
                            .SelectMany(e => e.Value.Errors.Select(x => new ValidationErrorResponse(e.Key, x.ErrorMessage)))
                            .ToList();

                        return new BadRequestObjectResult(
                            new MyResponseMessage("One or more validation errors occurred.", errors));
                    };
                })
                .AddFluentValidation(config =>
                {
                    config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

            return services;
        }
    }
}
