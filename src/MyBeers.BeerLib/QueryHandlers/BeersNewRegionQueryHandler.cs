using MyBeers.BeerLib.Api.Queries;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.QueryHandlers
{
    public class BeersNewRegionQueryHandler : BaseQueryHandler<Domain.Beer, BeersNewRegionQuery, IEnumerable<BeersNewRegionQuery.Beer>>
    {
        public BeersNewRegionQueryHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<BeersNewRegionQuery.Beer>> HandleAsync(BeersNewRegionQuery query)
        {
            var now = DateTime.Now;
            var beers = await Task.Run(() => Repository.AsQueryable().Where(x => x.SellStartDate > now && x.State == query.Region).ToList());


            return beers.Select(x => new BeersNewRegionQuery.Beer
            {
                State = x.State,
                Alcohol = x.AlcoholPercentage,
                City = x.City,
                Country = x.Country,
                Id = x.Id.ToString(),
                ImageUrl = x.ImageUrl,
                Name = x.Name,
                Price = x.Price,
                Producer = x.Producer,
                Type = x.Type
            });
        }
    }
}
 