using MyBeers.Common.CommonInterfaces;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System.Threading.Tasks;

namespace MyBeers.Common.Bases
{
    public abstract class BaseCommandHandler<TCommand, TDomain> : ICommandHandler<TCommand> where TCommand : ICommand where TDomain : IMongoEntity
    {
        protected IMongoRepository<TDomain> Repository { get; }
        protected IQueryDispatcher QueryDispatcher { get; }
        protected ICommandDispatcher CommandDispatcher { get; }
        public BaseCommandHandler(IMongoRepository<TDomain> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {
            Repository = repository;
            QueryDispatcher = queryDispatcher;
            CommandDispatcher = commandDispatcher;
        }
        public abstract Task HandleAsync(TCommand command);
    }
}
