using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MerosWebApi.Core.Models;
using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Repository;
using MerosWebApi.Persistence.Entites;
using MerosWebApi.Persistence.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

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
                .Eq( "_id", new ObjectId(meroId));
            var meros = await _dbService.Meros.FindAsync(fitler);
            var mero = meros.FirstOrDefault();

            if (mero == null)
                return null;

            var timePeriods = GetTimePeriodsAsync(mero.TimePeriods);

            var fields = mero.Fields
                .Select(f => FieldPropertyAssigner.MapFrom(f))
                .ToList();

            return Mero.CreateMero(mero.Id, mero.Name, mero.CreatorId, mero.CreatorEmail,
                mero.Description, await timePeriods, fields, mero.Files);
        }

        public async Task AddMeroAsync(Mero mero)
        {
            var dbMero = MeroPropertyAssigner.MapFrom(mero);

            await _dbService.Meros.InsertOneAsync(dbMero);
        }

        public async Task AddTimePeriodAsync(TimePeriod period)
        {
            var dbTimePeriod = TimePeriodPropertyAssigner.MapFrom(period);

            _dbService.TimePeriods.InsertOneAsync(dbTimePeriod);
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

                return meroDelResult.DeletedCount == 1 ?
                    new QuerryStatus(true, false, "Мероприятие успешно удаленно") 
                    : new QuerryStatus(false, false,
                        "Периоды мероприятия удалены, мероприятие не было удаленно");
            }

            return new QuerryStatus(false,false, "Мероприятие найдено, удаление безуспешно.");
        }
    }
}
