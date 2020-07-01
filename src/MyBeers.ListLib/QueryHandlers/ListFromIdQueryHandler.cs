using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Api.Queries;
using MyBeers.ListLib.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.ListLib.QueryHandlers
{
    public class ListFromIdQueryHandler : BaseQueryHandler<Domain.List, ListFromIdQuery, ListFromIdQuery.List>
    {
        public ListFromIdQueryHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<ListFromIdQuery.List> HandleAsync(ListFromIdQuery query)
        {
            var list = await Repository.FindByIdAsync(query.Id);

            var beers = await QueryDispatcher.DispatchAsync<BeersByIdsQuery, IEnumerable<BeersByIdsQuery.Beer>>(new BeersByIdsQuery { BeerIds = list.BeerIds });

            return new ListFromIdQuery.List
            {
                Id = list.Id.ToString(),
                Description = list.Description,
                Name = list.Name,
                Beers = beers.Select(x => new ListFromIdQuery.Beer
                {
                    Id = x.Id,
                    Name = x.Name,
                    Producer = x.Producer,
                    ImageUrl = x.ImageUrl,
                    SystemetProdId = x.SystemetProdId,
                    State = x.State,
                    Price = x.Price,
                    Country = x.Country,
                    City = x.City, 
                    Alcohol = x.Alcohol
                })
            };
        }
    }
}
