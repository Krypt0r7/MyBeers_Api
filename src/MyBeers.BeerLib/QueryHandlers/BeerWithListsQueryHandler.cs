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
            });

            return new BeerWithListsQuery.Beer
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
                Containers = beer.Containers.Select(x => new Api.Queries.Models.Container { Id = x.Id, Price = x.Price, ProductIdFromSystemet = x.ProductIdFromSystmet, RecycleFee = x.RecycleFee, SellStartDate = x.SellStartDate, Type = x.Type.ToString(), Volume = x.Volume, Ypk = x.Ypk}),
                Lists = lists.Select(list => new BeerWithListsQuery.List
                {
                    Id = list.Id,
                    Name = list.Name
                })
            };
        }
    }
}
