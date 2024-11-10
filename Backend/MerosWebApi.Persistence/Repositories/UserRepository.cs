using MerosWebApi.Core.Models;
using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Persistence.Helpers;

namespace MerosWebApi.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MerosDbContext _dbContext;

        public UserRepository(MerosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUser(User user)
        {
            var dbUser = PropertyAssigner.MapFrom(user);

            await _dbContext.Users.AddAsync(dbUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            var dbUser = await _dbContext.Users.FindAsync(userId);

            if (dbUser != null)
            {
                _dbContext.Users.Remove(dbUser);
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var dbUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            if (dbUser == null)
                return null;

            return PropertyAssigner.MapFrom(dbUser) ;
        }

        public async Task<User> GetUserById(Guid id)
        {
            var dbUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (dbUser == null)
                return null;

            return PropertyAssigner.MapFrom(dbUser);
        }

        public async Task<User> GetUserByUnconfirmedCode(string unconfirmedCode)
        {
            var dbUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.UnconfirmedEmailCode == unconfirmedCode);

            if (dbUser == null)
                return null;

            return PropertyAssigner.MapFrom(dbUser);
        }

        public async Task<User> GetUserByResetCode(string resetCode, string email)
        {
            var dbUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.ResetPasswordCode == resetCode &&
                                          u.Email == email);
            if (dbUser == null)
                return null;

            return PropertyAssigner.MapFrom(dbUser);
        }

        public async Task<User> UpdateUser(User user)
        {
            var dbUserToUpdate = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            PropertyAssigner.AssignPropertyValues(dbUserToUpdate, user);

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserByRefreshToken(string refreshToken)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null)
                return default;

            return PropertyAssigner.MapFrom(user);
        }
    }
}
