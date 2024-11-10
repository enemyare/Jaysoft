using MerosWebApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MerosWebApi.Core.Repository
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmail(string email);

        public Task<User> GetUserById(Guid id);

        public Task<User> GetUserByUnconfirmedCode(string unconfirmedCode);

        public Task<User> GetUserByResetCode(string resetCode, string email);

        public Task<User> GetUserByRefreshToken(string refreshToken);

        public Task AddUser(User user);

        public Task<User> UpdateUser(User user);

        public Task<bool> DeleteUser(Guid userId);
    }
}
