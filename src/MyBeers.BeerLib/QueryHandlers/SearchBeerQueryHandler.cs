using MyBeers.BeerLib.Api.Queries;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.QueryHandlers
{
    public class SearchBeerQueryHandler : BaseQueryHandler<Domain.Beer, SearchBeerQuery, IEnumerable<SearchBeerQuery.Beer>>
    {
        public SearchBeerQueryHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<SearchBeerQuery.Beer>> HandleAsync(SearchBeerQuery query)
        {
            var beers = await Task.Run(() => { return Repository.AsQueryable().Where(beer => beer.Name.ToLower().Contains(query.SearchString.ToLower()) || beer.Producer.ToLower().Contains(query.SearchString.ToLower())); });           

            var ordered = beers.OrderBy(x => x.Name).Take(40);

            return ordered.Select(x => new SearchBeerQuery.Beer
            {
                Alcohol = x.AlcoholPercentage,
                City = x.City,
                Country = x.Country,
                Id = x.Id.ToString(),
                Name = x.Name,
                Price = x.Price,
                Producer = x.Producer,
                State = x.State,
                ImageUrl = x.ImageUrl,
                Type = x.Type
            });
        }
    }
}
