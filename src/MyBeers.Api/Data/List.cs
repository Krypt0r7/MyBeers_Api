using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MyBeers.Api.Data
{
    public class List
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        public string Id { get; set; }
        public List<string> BeerIds { get; set; } = new List<string>();
        public string Owner { get; set; }
        public List<string> Collaborators { get; set; } = new List<string>();
    }
}
