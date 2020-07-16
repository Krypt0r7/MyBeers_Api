using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Api.Commands;
using MyBeers.ListLib.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.ListLib.CommandHandlers
{
    public class UpdateListCommandHandler : BaseCommandHandler<UpdateListCommand, Domain.List>
    {
        public UpdateListCommandHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(UpdateListCommand command)
        {

            var list = await Repository.FindByIdAsync(command.ListId);

            list.BeerIds = command.BeerIds.ToList();

            await Repository.ReplaceAsync(list);
        }
    }
}
