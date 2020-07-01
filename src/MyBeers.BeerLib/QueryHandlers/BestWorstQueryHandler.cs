using MyBeers.BeerLib.Api.Queries;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.RatingLib.Api.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.QueryHandlers
{
    public class BestWorstQueryHandler : BaseQueryHandler<Domain.Beer, BestWorstQuery, BestWorstQuery.BestWorst>
    {
        public BestWorstQueryHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override BestWorstQuery.BestWorst Handle(BestWorstQuery query)
        {
            var ratings = QueryDispatcher.Dispatch<RatingsBasicsQuery, IEnumerable<RatingsBasicsQuery.Rating>>(new RatingsBasicsQuery());

            var beerIds = ratings.Select(s => s.BeerId).Distinct().ToList();

            var beersQuery = Repository.AsQueryable().ToList();

            var beers = beersQuery.Where(x => beerIds.Contains(x.Id.ToString())).ToList();

            var grouped = ratings.GroupBy(x => x.BeerId).ToList();

            var beerCollection = new Dictionary<Beer, double>();

            foreach (var beer in grouped)
            {
                if (beer.Count() > 1)
                {
                    double average = 0;
                    foreach (var rating in beer)
                    {
                        average += rating.OverallRating;
                    }
                    average /= beer.Count();
                    average = System.Math.Round(average, 2);
                    beerCollection.Add(beers.First(f => f.Id.ToString() == beer.Key), average);
                }
            }
            var worstCol = beerCollection.OrderBy(x => x.Value).Take(5);
            
            var bestCol = beerCollection.OrderByDescending(x => x.Value).Take(5);

            return new BestWorstQuery.BestWorst
            {
                BestBeer = bestCol.Select(x => new BestWorstQuery.BestWorst.Beer
                {
                    Alcohol = x.Key.AlcoholPercentage,
                    AvarageRating = x.Value,
                    City = x.Key.City,
                    Country = x.Key.Country,
                    Id = x.Key.Id.ToString(),
                    ImageUrl = x.Key.ImageUrl,
                    Name = x.Key.Name,
                    Price = x.Key.Price,
                    Producer = x.Key.Producer,
                    State = x.Key.State,
                    Type = x.Key.Type
                }),
                WorstBeer = worstCol.Select(x => new BestWorstQuery.BestWorst.Beer 
                { 
                    Alcohol = x.Key.AlcoholPercentage, 
                    AvarageRating = x.Value, 
                    City = x.Key.City,
                    Country = x.Key.Country,
                    Id = x.Key.Id.ToString(),
                    ImageUrl = x.Key.ImageUrl,
                    Name = x.Key.Name,
                    Price = x.Key.Price,
                    Producer = x.Key.Producer,
                    State = x.Key.State,
                    Type = x.Key.Type
                })
            };

        }
    }
}
