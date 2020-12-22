using MyBeers.BeerLib.Domain;
using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.QueryHandlers
{
    public class BeersByIdsQueryHandler : BaseQueryHandler<Beer, BeersByIdsQuery, IEnumerable<BeersByIdsQuery.Beer>>
    {
        public BeersByIdsQueryHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<BeersByIdsQuery.Beer>> HandleAsync(BeersByIdsQuery query)
        {
            var beers = new List<Domain.Beer>();
            foreach (var item in query.BeerIds)
            {
                beers.Add(await Repository.FindByIdAsync(item));
            }
           
            return beers.Select(b => new BeersByIdsQuery.Beer 
            { 
                Id = b.Id.ToString(), 
                Alcohol = b.AlcoholPercentage, 
                City = b.City, 
                Country = b.Country, 
                Name = b.Name, 
                Producer = b.Producer, 
                State = b.State,
                ImageUrl = b.ImageUrl,
                Containers = b.Containers.Select(c => new BeersByIdsQuery.Container
                {
                    Price = c.Price,
                    Type = c.Type.ToString(),
                    Volume = c.Volume,
                    Ypk = c.Ypk
                })
            });
        }

   
    }
}
