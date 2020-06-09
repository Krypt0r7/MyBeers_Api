using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Api.Commands;
using MyBeers.ListLib.Domain;
using System;
using System.Collections.Generic;
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

            if (list.BeerIds.Contains(command.BeerId))
            {
                list.BeerIds.Remove(command.BeerId);
            }
            else
            {
                var beer = await QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = command.BeerId});

                list.BeerIds.Add(beer.Id);
            }

            await Repository.ReplaceAsync(list);
        }
    }
}
