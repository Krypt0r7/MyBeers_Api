using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Data
{
    public class Rating
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public int OverallRating { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string BeerId { get; set; }
    }
}
