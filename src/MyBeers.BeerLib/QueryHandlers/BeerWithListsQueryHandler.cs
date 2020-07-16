using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.QueryHandlers
{
    public class BeerWithListsQueryHandler : BaseQueryHandler<Domain.Beer, BeerWithListsQuery, BeerWithListsQuery.Beer>
    {
        public BeerWithListsQueryHandler(IMongoRepository<Domain.Beer> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<BeerWithListsQuery.Beer> HandleAsync(BeerWithListsQuery query)
        {
            var beer = await Repository.FindByIdAsync(query.Id);

            var lists = await QueryDispatcher.DispatchAsync<ListsFromBeerAndUserQuery, IEnumerable<ListsFromBeerAndUserQuery.List>>(new ListsFromBeerAndUserQuery 
            { 
                BeerId = query.Id, 
                UserId = query.UserId 
            });

            return new BeerWithListsQuery.Beer
            {
                Id = beer.Id.ToString(),
                AlcoholPercentage = beer.AlcoholPercentage,
                City = beer.City,
                Volume = beer.Volume,
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
                YPK = beer.YPK,
                Lists = lists.Select(list => new BeerWithListsQuery.List
                {
                    Id = list.Id,
                    Name = list.Name
                })
            };
        }
    }
}
