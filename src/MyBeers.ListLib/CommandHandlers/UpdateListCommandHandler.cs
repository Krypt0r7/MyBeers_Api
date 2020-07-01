using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Api.Commands;
using MyBeers.ListLib.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            list.BeerIds = list.BeerIds.Where(x => command.BeerIds.Contains(x)).ToList();

            var beersToAdd = command.BeerIds.Where(x => !list.BeerIds.Contains(x)).ToList();

            if (beersToAdd.Count > 0)
            {
                var beers = await QueryDispatcher.DispatchAsync<BeersByIdsQuery, IEnumerable<BeersByIdsQuery.Beer>>(new BeersByIdsQuery {BeerIds = beersToAdd });
                foreach (var beer in beers)
                {
                    list.BeerIds.Add(beer.Id);
                }
            }

            await Repository.ReplaceAsync(list);
        }
    }
}
