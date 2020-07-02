using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.RatingLib.Api.Queries;
using MyBeers.RatingLib.Domain;
using MyBeers.UserLib.Api.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.RatingLib.QueryHandlers
{
    public class RatingsQueryHandler : BaseQueryHandler<Domain.Rating, RatingsQuery, IEnumerable<RatingsQuery.Rating>>
    {
        public RatingsQueryHandler(IMongoRepository<Rating> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<RatingsQuery.Rating>> HandleAsync(RatingsQuery query)
        {
            var ratings = await Repository.FilterByAsync(filter => true);
            var ordered = ratings.OrderByDescending(o => o.Created)
                .Take(30);

            var userIds = ordered.Select(x => x.UserId).Distinct();
            var users = new List<RatingsQuery.User>();
            foreach (var id in userIds)
            {
                var userFetch = await QueryDispatcher.DispatchAsync<UserQuery, UserQuery.User>(new UserQuery { Id = id });
                users.Add(new RatingsQuery.User
                {
                    AvatarUrl = userFetch.AvatarUrl,
                    Id = userFetch.Id,
                    Username = userFetch.Username
                });
            }

            var ratingsNew = new List<RatingsQuery.Rating>();
            foreach (var rating in ordered)
            {
                var ratingNew = new RatingsQuery.Rating
                {
                    AfterTaste = rating.AfterTaste,
                    Chugability = rating.Chugability,
                    Description = rating.Description,
                    FirstImpression = rating.FirstImpression,
                    OverallRating = rating.OverallRating,
                    Taste = rating.Taste,
                    Value = rating.Value,
                    Id = rating.Id.ToString(),
                    User = users.FirstOrDefault(y => rating.UserId == y.Id),
                    Beer = await GetBeer(rating.BeerId)
                };
                ratingsNew.Add(ratingNew);
            }
            return ratingsNew;
        }

        private async Task<RatingsQuery.Beer> GetBeer(string id)
        {
            var beer = await QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = id });
            return new RatingsQuery.Beer
            {
                Alcohol = beer.AlcoholPercentage,
                Id = beer.Id,
                Name = beer.Name,
                Price = beer.Price,
                Producer = beer.Producer,
                Volume = beer.Volume
            };
        }
    }
}
