using MyBeers.Common.CommonInterfaces;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.Common.Bases
{
    public abstract class BaseQueryHandler<TDomain, TQuery, TResult> : IQueryHandler<TQuery, TResult> where TDomain : IMongoEntity where TQuery : IQuery<TResult>
    {
        protected IMongoRepository<TDomain> Repository { get; }
        protected IQueryDispatcher QueryDispatcher { get; }

        public BaseQueryHandler(IMongoRepository<TDomain> repository, IQueryDispatcher queryDispatcher)
        {
            Repository = repository;
            QueryDispatcher = queryDispatcher;
        }

        public abstract Task<TResult> HandleAsync(TQuery query);
    }
}
