using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MerosWebApi.Persistence.Entites
{
    public class DatabaseTimePeriod
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("start_time")]
        [BsonRequired]
        public DateTime StartTime { get; set; }

        [BsonElement("end_time")]
        [BsonRequired]
        public DateTime EndTime { get; set; }

        [BsonElement("total_places")]
        [BsonRequired]
        public int TotalPlaces { get; set; }
        
        [BsonElement("booked_places")]
        [BsonRequired]
        public int BookedPlaces { get; set; }
    }
}
