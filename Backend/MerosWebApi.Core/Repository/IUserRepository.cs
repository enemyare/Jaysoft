﻿using MerosWebApi.Core.Models.User;

namespace MerosWebApi.Core.Repository
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmail(string email);

        public Task<User> GetUserByVerificationCode(string uniqCode);

        public Task<User> GetUserByUnconfirmedEmailCode(string unconfirmedCode);

        public Task AddUser(User user);

        public Task<User> GetUserById(string id);

        public Task<User> GetUserByRefreshToken(string refreshToken);

        public Task<User> UpdateUser(User user);

        public Task<bool> DeleteUser(string userId);

        public Task<UserStatistic> GetUserStatisticById(string id);
    }
}
