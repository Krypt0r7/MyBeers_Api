using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.Common.MongoSettings
{
    public interface IMongoRepository<TDocument> where TDocument : IMongoEntity
    {
        IQueryable<TDocument> AsQueryable();

        Task<TDocument> FindByIdAsync(string id);

        IEnumerable<TDocument> FilterBy(
            Expression<Func<TDocument, bool>> filterExpression);

        IEnumerable<TProjected> FilterBy<TProjected>(
            Expression<Func<TDocument, bool>> filterExpression,
            Expression<Func<TDocument, TProjected>> projectionExpression);

        Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

        Task SaveAsync(TDocument document);

        Task ReplaceAsync(TDocument document);

        Task DeleteAsync(string id);
    }
}
