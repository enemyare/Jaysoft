﻿using MerosWebApi.Application.Common.SecurityHelpers;

namespace MerosWebApi.Application.Common.DTOs.UserService
{
    public class LogInResult
    {
        public AuthenticationResDto AuthenticationResDto { get; }

        public string AccessToken { get; }

        public RefreshToken RefreshToken { get; }

        public LogInResult(AuthenticationResDto resDto, string accessToken, RefreshToken refreshToken)
        {
            AuthenticationResDto = resDto;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
