using MyBeers.BeerLib.Domain;
using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace MyBeers.BeerLib.beerHandlers
{
    public class BeersQueryHandler : BaseQueryHandler<Beer, BeersQuery, IEnumerable<BeersQuery.Beer>>
    {
        public BeersQueryHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<BeersQuery.Beer>> HandleAsync(BeersQuery query)
        {
            var beers = Repository.AsQueryable();

            return beers.Select(beer => new BeersQuery.Beer
            {
                Id = beer.Id.ToString(),
                AlcoholPercentage = beer.AlcoholPercentage,
                City = beer.City,
                Country = beer.Country,
                ImageUrl = beer.ImageUrl,
                Name = beer.Name,
                Producer = beer.Producer,
                State = beer.State,
                Style = beer.Style,
                Type = beer.Type,
                Containers = beer.Containers.Select(c => new BeersQuery.Beer.Container
                {
                    Price = c.Price,
                    ProductIdFromSystemet = c.ProductIdFromSystmet,
                    RecycleFee = c.RecycleFee,
                    SellStartDate = c.SellStartDate,
                    Type = c.Type.ToString(),
                    Volume = c.Volume,
                    Ypk = c.Ypk
                })
            });
        }
    }
}
