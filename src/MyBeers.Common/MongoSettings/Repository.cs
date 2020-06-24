using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyBeers.Common.MongoSettings
{
    public class Repository<TDocument> : IMongoRepository<TDocument> where TDocument : IMongoEntity
    {
        private readonly IMongoCollection<TDocument> _collection;
        public Repository(IDBSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
        }

        private protected string GetCollectionName(Type docType)
        {
            return ((BsonCollectioinAttribute)docType.GetCustomAttributes(
                typeof(BsonCollectioinAttribute), true).FirstOrDefault())?.CollectionName;
        }
        public IQueryable<TDocument> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        public IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToList();
        }

        public TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).FirstOrDefault();
        }

        public Task SaveAsync(TDocument document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }


        public async Task ReplaceAsync(TDocument document)
        {
            var filter = Builders<TDocument>.Filter.Eq(x => x.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public TDocument FindById(string id)
        {
            var objectId = new ObjectId(id);
            return _collection.Find(f => f.Id == objectId).FirstOrDefault();
        }
    }
}
