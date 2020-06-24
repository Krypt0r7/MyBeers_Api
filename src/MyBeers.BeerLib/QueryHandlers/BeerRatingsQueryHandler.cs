using MyBeers.BeerLib.Api.Queries;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.RatingLib.Api.Queries;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.QueryHandlers
{
    public class BeerRatingsQueryHandler : BaseQueryHandler<Domain.Beer, BeerRatingsQuery, BeerRatingsQuery.Beer>
    {
        public BeerRatingsQueryHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override BeerRatingsQuery.Beer Handle(BeerRatingsQuery query)
        {
            var beer = Repository.FindById(query.Id);

            var ratings =  QueryDispatcher.Dispatch<RatingsByBeerQuery, IEnumerable<RatingsByBeerQuery.Rating>>(new RatingsByBeerQuery { BeerId = query.Id });

            var orderedRatings = ratings.OrderByDescending(x => x.Created);

            return new BeerRatingsQuery.Beer
            {
                Id = beer.Id.ToString(),
                Name = beer.Name,
                Price = beer.Price,
                Producer = beer.Producer,
                ImageUrl = beer.ImageUrl,
                Ratings = orderedRatings.Select(x => new BeerRatingsQuery.Rating
                {
                    AfterTaste = x.AfterTaste,
                    Chugability = x.Chugability,
                    Description = x.Description,
                    FirstImpression = x.FirstImpression,
                    OverallRating = x.OverallRating,
                    Taste = x.Taste,
                    Value = x.Value,
                    User = new BeerRatingsQuery.User { AvatarUrl = x.User.AvatarUrl, Username = x.User.Username, Id = x.User.Id }
                })
            };
        }
    }
}
