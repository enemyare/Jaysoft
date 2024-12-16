using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Models.PhormAnswer;
using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Entites;
using MerosWebApi.Persistence.Helpers;
using MerosWebApi.Persistence.Repositories.MyDbExceptions;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace MerosWebApi.Persistence.Repositories
{
    public class MeroRepository : IMeroRepository
    {
        private readonly MongoDbService _dbService;

        public MeroRepository(MongoDbService dbContext)
        {
            _dbService = dbContext;
        }

        public async Task<Mero> GetMeroByIdAsync(string meroId)
        {
            var fitler = Builders<DatabaseMero>.Filter
                .Eq("_id", new ObjectId(meroId));
            var meros = await _dbService.Meros.FindAsync(fitler);
            var mero = meros.FirstOrDefault();

            if (mero == null)
                return null;

            var timePeriods = GetTimePeriodsAsync(mero.TimePeriods);

            var fields = mero.Fields
                .Select(f => FieldPropertyAssigner.MapFrom(f))
                .ToList();

            return Mero.CreateMero(mero.Id, mero.UniqueInviteCode, mero.Name, mero.CreatorId, mero.CreatorEmail,
                mero.Description, await timePeriods, fields, mero.Files);
        }

        public async Task<Mero> GetMeroByInviteCodeAsync(string uniqueInviteCode)
        {
            var fitler = Builders<DatabaseMero>.Filter
                .Eq("uniq_inv_code", uniqueInviteCode);
            var meros = await _dbService.Meros.FindAsync(fitler);
            var mero = meros.FirstOrDefault();

            if (mero == null)
                return null;

            var timePeriods = GetTimePeriodsAsync(mero.TimePeriods);

            var fields = mero.Fields
                .Select(f => FieldPropertyAssigner.MapFrom(f))
                .ToList();

            return Mero.CreateMero(mero.Id, mero.UniqueInviteCode, mero.Name, mero.CreatorId, mero.CreatorEmail,
                mero.Description, await timePeriods, fields, mero.Files);
        }

        public async Task<List<Mero>> GetListMerosWhereCreator(int startIndex, int count, string creatorId)
        {
            var phormAnswers = await GetListMerosAsync(p => p.CreatorId == creatorId, startIndex, count);

            return await TransformMeros(phormAnswers);
        }


        public async Task<List<Mero>> GetListMerosWhereUser(int startIndex, int count, string userId)
        {
            var phormAnswers = await GetListPhormAnswersAsync(p => p.UserId == userId, startIndex, count);
            return await GetListMerosForPhormAnswers(phormAnswers);
        }

        public async Task AddMeroAsync(Mero mero)
        {
            var dbMero = MeroPropertyAssigner.MapFrom(mero);

            await _dbService.Meros.InsertOneAsync(dbMero);
        }

        public async Task AddTimePeriodAsync(TimePeriod period)
        {
            var dbTimePeriod = TimePeriodPropertyAssigner.MapFrom(period);

            await _dbService.TimePeriods.InsertOneAsync(dbTimePeriod);
        }

        public async Task<bool> AddMeroPhormAnswerAsync(PhormAnswer phormAnswer)
        {
            using (var session = await _dbService.PhormAnswers.Database.Client.StartSessionAsync())
            {
                //session.StartTransaction();

                try
                {
                    // Получаем информацию о периоде времени
                    var timePeriod = await _dbService.TimePeriods
                        .Find(session, tp => tp.Id == phormAnswer.TimePeriod.Id)
                        .FirstOrDefaultAsync();

                    if (timePeriod == null)
                    {
                        throw new TransactionLogicException("Time period not found.");
                    }

                    // Проверяем доступные места
                    if (timePeriod.BookedPlaces >= timePeriod.TotalPlaces)
                    {
                        throw new TransactionLogicException("No available places.");
                    }

                    // Создаем новый ответ
                    var newAnswer = new DatabasePhormAnswer
                    {
                        MeroId = phormAnswer.MeroId,
                        UserId = phormAnswer.UserId,
                        Answers = phormAnswer.Answers.Select(a => new DatabaseAnswer
                        {
                            QuestionAnswers = a.QuestionAnswers,
                            QuestionText = a.QuestionText
                        })
                        .ToList(),
                        TimePeriodId = phormAnswer.TimePeriod.Id,
                        CreatedTime = DateTime.Now
                    };

                    // Добавляем новый ответ в коллекцию
                    await _dbService.PhormAnswers.InsertOneAsync(session, newAnswer);

                    // Увеличиваем количество забронированных мест
                    var updateDefinition = Builders<DatabaseTimePeriod>
                        .Update.Inc(tp => tp.BookedPlaces, 1);

                    await _dbService.TimePeriods
                        .UpdateOneAsync(session, tp => tp.Id == phormAnswer.TimePeriod.Id, updateDefinition);

                    // Завершаем транзакцию
                    //await session.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    // Откатываем транзакцию в случае ошибки
                    //await session.AbortTransactionAsync();
                    Console.WriteLine($"Transaction aborted: {ex.Message}");
                    return false; // Неудача
                }
            }

            return true;
        }

        public async Task<PhormAnswer> GetMeroPhormAnswerByIdAsync(string phormId)
        {
            var findQuerry = await _dbService.PhormAnswers
                .FindAsync(p => p.Id == phormId);

            var phormAnswer = findQuerry.FirstOrDefault();

            if (phormAnswer == null)
                return null;

            var answers = phormAnswer.Answers
                .Select(a => new Answer(a.QuestionText, a.QuestionAnswers))
                .ToList();

            var timePeriods = await GetTimePeriodsAsync(new[] { phormAnswer.TimePeriodId });
            var period = timePeriods.FirstOrDefault();

            return PhormAnswer.Create(phormAnswer.Id, phormAnswer.MeroId, phormAnswer.UserId,
                answers, period, phormAnswer.CreatedTime);
        }

        public async Task<List<PhormAnswer>> GetListMeroPhormAnswersByMeroAsync(int startIndex, int count, string meroId)
        {
            var phormAnswers = await GetListPhormAnswersAsync(p => p.MeroId == meroId, startIndex, count);

            return TransformPhormAnswers(phormAnswers);
        }

        public async Task<List<TimePeriod>> GetTimePeriodsAsync(IEnumerable<string> ids)
        {
            var periodFilter = Builders<DatabaseTimePeriod>.Filter
                .In(doc => doc.Id, ids);
            var dbTimePeridos = await _dbService.TimePeriods
                .FindAsync(periodFilter);

            var timePeriods = dbTimePeridos.ToEnumerable()
                .Select(period => TimePeriodPropertyAssigner.MapFrom(period))
                .ToList();

            return timePeriods;
        }

        public async Task<QuerryStatus> DeleteMeroByIdAsync(string meroId)
        {
            var fitler = Builders<DatabaseMero>.Filter
                .Eq("_id", new ObjectId(meroId));

            var meros = await _dbService.Meros.FindAsync(fitler);
            var mero = meros.FirstOrDefault();

            //Удалить все периоды и мероприятие mero.TimePeriods
            var timePeriodsFilter = Builders<DatabaseTimePeriod>.Filter
                .In(doc => doc.Id, mero.TimePeriods);

            var periodsDelResult = await _dbService.TimePeriods.DeleteManyAsync(timePeriodsFilter);

            if (periodsDelResult.DeletedCount > 0)
            {
                var meroDelResult = await _dbService.Meros.DeleteOneAsync(fitler);

                return meroDelResult.DeletedCount == 1
                    ? new QuerryStatus(true, false, "Мероприятие успешно удаленно")
                    : new QuerryStatus(false, false,
                        "Периоды мероприятия удалены, мероприятие не было удаленно");
            }

            return new QuerryStatus(false, false, "Мероприятие найдено, удаление безуспешно.");
        }

        #region Helpers

        private async Task<List<DatabasePhormAnswer>> GetListPhormAnswersAsync(Expression<Func<DatabasePhormAnswer, bool>> filter, int startIndex, int count)
        {
            return await _dbService.PhormAnswers
                .Find(filter)
                .Skip(startIndex)
                .Limit(count)
                .ToListAsync();
        }

        private List<PhormAnswer> TransformPhormAnswers(List<DatabasePhormAnswer> phormAnswers)
        {
            var result = new List<PhormAnswer>();
            foreach (var phormAnswer in phormAnswers)
            {
                var answers = phormAnswer.Answers
                    .Select(a => new Answer(a.QuestionText, a.QuestionAnswers))
                    .ToList();

                var timePeriods = GetTimePeriodsAsync(new[] { phormAnswer.TimePeriodId }).Result;
                var period = timePeriods.FirstOrDefault();

                result.Add(PhormAnswer.Create(phormAnswer.Id, phormAnswer.MeroId, phormAnswer.UserId,
                    answers, period, phormAnswer.CreatedTime));
            }

            return result;
        }

        private async Task<List<DatabaseMero>> GetListMerosAsync(Expression<Func<DatabaseMero, bool>> filter, int startIndex, int count)
        {
            return await _dbService.Meros
                .Find(filter)
                .Skip(startIndex)
                .Limit(count)
                .ToListAsync();
        }

        private async Task<List<Mero>> TransformMeros(List<DatabaseMero> dbMeros)
        {
            var result = new List<Mero>();
            foreach (var dbmero in dbMeros)
            {
                var timePeriods = GetTimePeriodsAsync(dbmero.TimePeriods);

                var fields = dbmero.Fields
                    .Select(f => FieldPropertyAssigner.MapFrom(f))
                    .ToList();

                result.Add(Mero.CreateMero(dbmero.Id, dbmero.UniqueInviteCode, dbmero.Name, dbmero.CreatorId,
                    dbmero.CreatorEmail, dbmero.Description, await timePeriods, fields, dbmero.Files));
            }

            return result;
        }

        private async Task<List<Mero>> GetListMerosForPhormAnswers(IEnumerable<DatabasePhormAnswer> phormAnswers)
        {
            var meros = new List<Mero>();
            foreach (var phormAnswer in phormAnswers)
            {
                var mero = await GetMeroByIdAsync(phormAnswer.MeroId);
                if (mero != null)
                {
                    var timePeriod = mero.TimePeriods
                        .Where(p => p.Id == phormAnswer.TimePeriodId)
                        .ToList();

                    var processedMero = Mero.CreateMero(mero.Id, mero.UniqueInviteCode, mero.Name, mero.CreatorId,
                        mero.CreatorEmail, mero.Description, mero.TimePeriods, mero.Fields, new List<MeroFile>());

                    meros.Add(mero);
                }
            }
            return meros;
        }

        #endregion
    }
}
