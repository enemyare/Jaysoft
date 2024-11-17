using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MerosWebApi.Persistence.Entites
{
    public class DatabaseMero
    {
        [BsonId]
        [BsonElement("_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("name")]
        [BsonRequired]
        public string Name { get; set; }

        [BsonElement("creator_id")]
        [BsonRequired]
        public string CreatorId { get; set; }

        [BsonElement("creator_email")]
        [BsonRequired]
        public string CreatorEmail { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("time_periods")]
        [BsonRequired]
        public List<string> TimePeriods { get; set; }

        [BsonElement("fields")]
        [BsonRequired]
        public List<DatabaseField> Fields { get; set; }

        [BsonElement("files")]
        public List<MeroFile> Files { get; set; }
    }
}
