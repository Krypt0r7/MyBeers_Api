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
           
            return beers.Select(x => new BeersByIdsQuery.Beer 
            { 
                Id = x.Id.ToString(), 
                Alcohol = x.AlcoholPercentage, 
                City = x.City, 
                Country = x.Country, 
                Name = x.Name, 
                Price = x.Price, 
                Producer = x.Producer, 
                State = x.State,
                ImageUrl = x.ImageUrl,
                SystemetProdId = x.ProductIdSystemet
            });
        }

   
    }
}
