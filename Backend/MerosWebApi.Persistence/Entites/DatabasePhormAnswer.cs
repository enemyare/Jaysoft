﻿using MerosWebApi.Core.Models.Mero;
using MerosWebApi.Core.Models.PhormAnswer;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MerosWebApi.Persistence.Entites
{
    public class DatabasePhormAnswer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("mero_id")]
        [BsonRequired]
        public string MeroId { get; set; }

        [BsonElement("user_id")]
        [BsonRequired]
        public string UserId { get; set; }

        [BsonElement("answers")]
        [BsonRequired]
        public List<DatabaseAnswer> Answers { get; set; }

        [BsonElement("period_id")]
        [BsonRequired]
        public string TimePeriod { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedTime { get; set; }
    }
}
