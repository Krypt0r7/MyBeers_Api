using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Data
{
    public class Beer
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime Added { get; set; }
        public bool Favourite { get; set; }
        public double YPK { get; set; }
        public BeerData BeerData { get; set; }
        public string UserId { get; set; }
    }
}
