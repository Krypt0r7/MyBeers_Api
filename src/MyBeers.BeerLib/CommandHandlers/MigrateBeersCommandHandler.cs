using MyBeers.BeerLib.Api.Commands;
using MyBeers.BeerLib.Api.Queries;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.Extensions;
using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.CommandHandlers
{
    public class MigrateBeersCommandHandler : BaseCommandHandler<MigrateBeersCommand, Domain.Beer>
    {
        public MigrateBeersCommandHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(MigrateBeersCommand command)
        {
            var beers = await QueryDispatcher.DispatchAsync<BeersQuery, IEnumerable<BeersQuery.Beer>>(new BeersQuery());

            foreach (var beer in beers)
            {
                var containers = beer.Containers.Select(x => 
                    new Container(Guid.NewGuid().ToString(), ContainerConvert(x.Type).ToEnum<ContainerType>(), x.Volume, x.Price, x.RecycleFee, x.Ypk, x.SellStartDate, x.ProductIdFromSystemet))
                    .ToList();

                var newBeer = new Beer(beer.Id, beer.Name, beer.Producer, beer.AlcoholPercentage,
                    beer.Country, beer.State, beer.City, beer.Type, beer.Style, beer.ImageUrl,
                    true, new List<string>(), containers);

                await Repository.SaveAsync(newBeer);
            }
        }

        private string ContainerConvert(string value)
        {
            switch (value)
            {
                case "Fat":
                    return "Keg";
                case "Burk":
                    return "Can";
                default:
                    return "Bottle";
            }
        }
    }
}
