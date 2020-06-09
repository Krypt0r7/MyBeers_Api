using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Api.Commands;
using MyBeers.ListLib.Domain;
using System.Threading.Tasks;

namespace MyBeers.ListLib.CommandHandlers
{
    public class CreateListCommandHandler : BaseCommandHandler<CreateListCommand, Domain.List>
    {
        public CreateListCommandHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(CreateListCommand command)
        {
            
            var list = new Domain.List { OwnerId = command.UserId, Name = command.Name, Description = command.Description };

            await Repository.SaveAsync(list);
        }
    }
}
