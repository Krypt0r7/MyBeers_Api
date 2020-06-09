using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MyBeers.Common.MongoSettings
{
    public class MongoEntity : IMongoEntity
    {
        public ObjectId Id { get; set; }

        public DateTime Created => Id.CreationTime;
    }

    public interface IMongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }
        DateTime Created { get; }
    }
}
