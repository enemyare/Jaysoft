using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MerosWebApi.Core.Models;
using MerosWebApi.Persistence.Entites;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core;

namespace MerosWebApi.Persistence
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;
        public IMongoCollection<DatabaseUser> Users { get; set; } 

        public IMongoCollection<DatabaseMero> Meros {get; set; }

        public IMongoCollection<DatabaseTimePeriod> TimePeriods { get; set; }

        public IMongoCollection<DatabasePhormAnswer> PhormAnswers { get; set; }

        public MongoDbService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            MongoClient client = new MongoClient(mongoDbSettings.Value.ConnectionURI);
            var _database = client.GetDatabase(mongoDbSettings.Value.DatabaseName);

            Meros = _database.GetCollection<DatabaseMero>("meros");
            Users = _database.GetCollection<DatabaseUser>("users");
            TimePeriods = _database.GetCollection<DatabaseTimePeriod>("time_periods");
            PhormAnswers = _database.GetCollection<DatabasePhormAnswer>("phorm_answers");
        }
    }
}
