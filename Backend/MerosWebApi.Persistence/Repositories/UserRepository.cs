using MerosWebApi.Core.Models.User;
using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Entites;
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
            var filter = Builders<DatabaseUser>.Filter.Eq("_id", new ObjectId(userId));
            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();

            if (dbUser != null)
            {
                await _dbService.Users.DeleteOneAsync(filter);
                return true;
            }

            return false;
        }

        public async Task<UserStatistic> GetUserStatisticById(string userId)
        {
            var now = DateTime.Now;

            // Считаем количество созданных пользователем мероприятий
            var createdMeros = await GetCreatedMeros(userId);

            // Найдем временные интервалы для каждого созданного мероприятия
            var timePeriods = await GetTimePeriodsForMeros(createdMeros);

            // Отфильтруем завершенные временные интервалы и посчитаем сумму забронированных мест
            var participantsCount = CalculateParticipantsCount(timePeriods, now);

            // Загрузим временные интервалы для посещённых мероприятий
            var visitedTimePeriods = await GetCompletedVisitedTimePeriods(userId, now);

            // Посчитаем количество посещённых мероприятий
            var userRegisteredMerosCount = visitedTimePeriods.Count();

            return new UserStatistic
            {
                CreatedMerosCount = createdMeros.Count,
                ParticipantsCount = participantsCount,
                UserRegistredMerosCount = userRegisteredMerosCount
            };
        }

        private async Task<List<DatabaseMero>> GetCreatedMeros(string userId)
        {
            var createdMerosFilter = Builders<DatabaseMero>.Filter.Eq(m => m.CreatorId, userId);
            return await _dbService.Meros.Find(createdMerosFilter).ToListAsync();
        }

        private async Task<List<DatabaseTimePeriod>> GetTimePeriodsForMeros(List<DatabaseMero> meros)
        {
            var timePeriodIds = meros.SelectMany(m => m.TimePeriods).ToArray();
            var timePeriodsFilter = Builders<DatabaseTimePeriod>.Filter.In(tp => tp.Id, timePeriodIds);
            return await _dbService.TimePeriods.Find(timePeriodsFilter).ToListAsync();
        }

        private int CalculateParticipantsCount(IEnumerable<DatabaseTimePeriod> timePeriods, DateTime now)
        {
            var completedTimePeriods = timePeriods.Where(tp => tp.EndTime < now);
            return completedTimePeriods.Sum(tp => tp.BookedPlaces);
        }

        private async Task<List<DatabaseTimePeriod>> GetCompletedVisitedTimePeriods(string userId, DateTime now)
        {
            var visitedPhormAnswersFilter = Builders<DatabasePhormAnswer>.Filter.Eq(pa => pa.UserId, userId);
            var visitedPhormAnswers = await _dbService.PhormAnswers.Find(visitedPhormAnswersFilter).ToListAsync();

            var visitedTimePeriodIds = visitedPhormAnswers.Select(pa => pa.TimePeriodId).ToArray();
            var visitedTimePeriodsFilter = Builders<DatabaseTimePeriod>.Filter.In(tp => tp.Id, visitedTimePeriodIds);
            var visitedTimePeriods = await _dbService.TimePeriods.Find(visitedTimePeriodsFilter).ToListAsync();

            return visitedTimePeriods.Where(tp => tp.EndTime < now).ToList();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var filter = Builders<DatabaseUser>.Filter.Eq("email", email);
            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();
            if (dbUser == null)
                return null;

            return UserPropertyAssighner.MapFrom(dbUser);
        }

        public async Task<User> GetUserByVerificationCode(string uniqCode)
        {
            var dbUsers = await _dbService.Users
                .FindAsync(u => u.VerificationCode == uniqCode);

            var dbUser = dbUsers.FirstOrDefault();

            return dbUser == null ? null : UserPropertyAssighner.MapFrom(dbUser);
        }

        public async Task<User> GetUserById(string id)
        {
            var filter = Builders<DatabaseUser>.Filter.Eq("_id", new ObjectId(id));
            var dbUsers = await _dbService.Users.FindAsync(filter);
            var dbUser = dbUsers.FirstOrDefault();

            if (dbUser == null)
                return null;

            return UserPropertyAssighner.MapFrom(dbUser);
        }

        public async Task<User> GetUserByUnconfirmedEmailCode(string unconfirmedCode)
        {
            var filter = Builders<DatabaseUser>.Filter.Eq("unconf_email_code", unconfirmedCode);

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
