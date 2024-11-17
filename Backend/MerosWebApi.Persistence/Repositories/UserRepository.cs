using MerosWebApi.Core.Models;
using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Persistence.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MerosWebApi.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoDbService _dbService;

        public UserRepository(MongoDbService dbContext)
        {
            _dbService = dbContext;
        }

        public async Task AddUser(User user)
        {
            var dbUser = UserPropertyAssighner.MapFrom(user);

            await _dbService.Users.InsertOneAsync(dbUser);
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var filter = Builders<DatabaseUser>.Filter.Eq( "_id", new ObjectId(userId));
            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();

            if (dbUser != null)
            {
                await _dbService.Users.DeleteOneAsync(filter);
                return true;
            }

            return false;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var filter = Builders<DatabaseUser>.Filter.Eq("email", email);
            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();
            if (dbUser == null)
                return null;

            return UserPropertyAssighner.MapFrom(dbUser) ;
        }

        public async Task<User> GetUserById(string id)
        {
            var filter = Builders<DatabaseUser>.Filter.Eq("_id", new ObjectId(id) );
            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();

            if (dbUser == null)
                return null;

            return UserPropertyAssighner.MapFrom(dbUser);
        }

        public async Task<User> GetUserByUnconfirmedCode(string unconfirmedCode)
        {
            var filter = Builders<DatabaseUser>.Filter.Eq("unconf_email_code", unconfirmedCode);

            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();

            if (dbUser == null)
                return null;

            return UserPropertyAssighner.MapFrom(dbUser);
        }

        public async Task<User> GetUserByResetCode(string resetCode, string email)
        {
            var builder = Builders<DatabaseUser>.Filter;

            var filter = builder.Eq("reset_pwd_code", resetCode) & builder.Eq("email", email);

            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();

            if (dbUser == null)
                return null;

            return UserPropertyAssighner.MapFrom(dbUser);
        }

        public async Task<User> UpdateUser(User user)
        {
            var filter = Builders<DatabaseUser>.Filter.Eq("_id", new ObjectId(user.Id));
            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();

            UserPropertyAssighner.AssignPropertyValues(dbUser, user);

            var updateResult = await _dbService.Users.ReplaceOneAsync(filter, dbUser);

            if (updateResult.ModifiedCount == 0)
            {
                throw new Exception("User update failed");
            }

            return user;
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            var filter = Builders<DatabaseUser>.Filter.Eq("refresh_token", refreshToken);
            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();

            if (dbUser == null)
                return default;

            return UserPropertyAssighner.MapFrom(dbUser);
        }
    }
}
