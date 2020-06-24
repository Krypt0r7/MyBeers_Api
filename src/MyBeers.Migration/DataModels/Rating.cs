using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.Migration.DataModels
{
    public class Rating
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedTime { get; set; }
        public double OverallRating { get; set; }
        public int Taste { get; set; }
        public int AfterTaste { get; set; }
        public int Chugability { get; set; }
        public int Value { get; set; }
        public int FirstImpression { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string BeerId { get; set; }
    }
}
