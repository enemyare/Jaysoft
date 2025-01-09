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
            using (var session = await _dbService.Client.StartSessionAsync())
            {
                session.StartTransaction();

                try
                {
                    var timePeriod = await _dbService.TimePeriods
                        .Find(session, tp => tp.Id == phormAnswer.TimePeriod.Id)
                        .FirstOrDefaultAsync();

                    if (timePeriod == null)
                        throw new TransactionLogicException("Период записи не найден");

                    if (timePeriod.BookedPlaces >= timePeriod.TotalPlaces)
                        throw new TransactionLogicException("Нет доступных мест");

                    var newAnswer = new DatabasePhormAnswer
                    {
                        Id = phormAnswer.Id,
                        MeroId = phormAnswer.MeroId,
                        UserId = phormAnswer.UserId,
                        Answers = phormAnswer.Answers.Select(a => new DatabaseAnswer
                        {
                            QuestionAnswer = a.QuestionAnswer,
                            QuestionText = a.QuestionText
                        }).ToList(),
                        TimePeriodId = phormAnswer.TimePeriod.Id,
                        CreatedTime = DateTime.Now
                    };

                    await _dbService.PhormAnswers.InsertOneAsync(session, newAnswer);
                    var updateDefinition = Builders<DatabaseTimePeriod>
                        .Update.Inc(tp => tp.BookedPlaces, 1);

                    var updateResult = await _dbService.TimePeriods
                        .UpdateOneAsync(session, tp => tp.Id == phormAnswer.TimePeriod.Id && tp.BookedPlaces < tp.TotalPlaces, updateDefinition);

                    if (updateResult.ModifiedCount == 0)
                        throw new TransactionLogicException("Ошибка бронирования, возможно, место уже забронировано другим");

                    await session.CommitTransactionAsync();
                }
                catch (Exception ex)
                {
                    // Откатываем транзакцию в случае ошибки
                    await session.AbortTransactionAsync();
                    Console.WriteLine($"Transaction aborted: {ex.Message}");
                    return false;
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
                .Select(a => new Answer(a.QuestionText, a.QuestionAnswer))
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

        public async Task<QuerryStatus> DeleteMeroAsync(Mero mero)
        {
            using (var session = await _dbService.Client.StartSessionAsync())
            {
                session.StartTransaction();

                try
                {
                    var filter = Builders<DatabaseMero>.Filter
                        .Eq("_id", new ObjectId(mero.Id));

                    var timePeriodsFilter = Builders<DatabaseTimePeriod>.Filter
                        .In("_id", mero.TimePeriods.Select(t => new ObjectId(t.Id)));

                    var periodsCount = await _dbService.TimePeriods.CountDocumentsAsync(timePeriodsFilter);
                    var meroDelResult = await _dbService.Meros.DeleteOneAsync(filter);

                    if (meroDelResult.DeletedCount == 1)
                    {
                        var periodsDelResult = await _dbService.TimePeriods.DeleteManyAsync(timePeriodsFilter);

                        if (periodsDelResult.DeletedCount == periodsCount)
                        {
                            await session.CommitTransactionAsync();
                            return new QuerryStatus(true, false, "Все мероприятия успешно удалены");
                        }
                        await session.AbortTransactionAsync();
                        return new QuerryStatus(false, false, "Мероприятие удалено, но не все периоды.");
                    }
                    await session.AbortTransactionAsync();
                    return new QuerryStatus(false, false, "Мероприятие не найдено для удаления.");
                }
                catch (Exception ex)
                {
                    await session.AbortTransactionAsync();
                    return new QuerryStatus(false, true, $"Ошибка при удалении: {ex.Message}");
                }
            }
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
                    .Select(a => new Answer(a.QuestionText, a.QuestionAnswer))
                    .ToList();

                var timePeriods = GetTimePeriodsAsync(new[] { phormAnswer.TimePeriodId }).Result;
                var period = timePeriods.FirstOrDefault();

                result.Add(PhormAnswer.Create(phormAnswer.Id, phormAnswer.MeroId, phormAnswer.UserId,
                    answers, period, phormAnswer.CreatedTime));
            }

            return result;
        }

        private List<DatabaseTimePeriod> TransformTimePeriods(List<TimePeriod> timePeriods)
        {
            return timePeriods.Select(t => new DatabaseTimePeriod
            {
                Id = t.Id,
                StartTime = t.StartTime,
                BookedPlaces = t.BookedPlaces,
                TotalPlaces = t.TotalPlaces
            }).ToList();
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
