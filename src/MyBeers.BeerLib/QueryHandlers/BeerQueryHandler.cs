using MyBeers.BeerLib.Domain;
using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.beerHandlers
{
    public class BeerbeerHandler : BaseQueryHandler<Domain.Beer, BeerQuery, BeerQuery.Beer>
    {
        public BeerbeerHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<BeerQuery.Beer> HandleAsync(BeerQuery query)
        {
            var beer = await Repository.FindByIdAsync(query.Id);

            if (beer == null)
                throw new Exception("Beer not found");

            return new BeerQuery.Beer
            {
                Id = beer.Id.ToString(),
                AlcoholPercentage = beer.AlcoholPercentage,
                City = beer.City,
                Container = beer.Container,
                Country = beer.Country,
                ImageUrl = beer.ImageUrl,
                Name = beer.Name,
                Price = beer.Price,
                Producer = beer.Producer,
                ProductIdSystemet = beer.ProductIdSystemet,
                ProductionScale = beer.ProductionScale,
                RecycleFee = beer.RecycleFee,
                State = beer.State,
                Style = beer.Style,
                Taste = beer.Taste,
                Type = beer.Type,
                Usage = beer.Usage,
                Volume = beer.Volume,
                YPK = beer.YPK
            };
        }
    }
}
