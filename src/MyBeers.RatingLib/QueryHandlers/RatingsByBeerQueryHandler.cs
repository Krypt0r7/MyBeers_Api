using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.RatingLib.Domain;
using MyBeers.RatingLib.Api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBeers.BeerLib.Api.Queries;
using MyBeers.UserLib.Api.Queries;

namespace MyBeers.RatingLib.QueryHandlers
{
    public class RatingsByBeerQueryHandler : BaseQueryHandler<Domain.Rating, RatingsByBeerQuery, IEnumerable<RatingsByBeerQuery.Rating>>
    {
        public RatingsByBeerQueryHandler(IMongoRepository<Rating> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<RatingsByBeerQuery.Rating>> HandleAsync(RatingsByBeerQuery query)
        {
            var beer = await QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = query.BeerId });

            if (beer == null)
                throw new Exception("Beer not found");

            var ratings = Repository.FilterBy(filter => filter.BeerId == query.BeerId);

            return ratings.Select(x => new RatingsByBeerQuery.Rating
            {
                AfterTaste = x.AfterTaste,
                Chugability = x.Chugability,
                Description = x.Description,
                FirstImpression = x.FirstImpression,
                Id = x.Id.ToString(),
                OverallRating = x.OverallRating,
                Taste = x.Taste,
                Value = x.Value,
                Created = x.Created,
                User = GetUser(x.UserId)
            });

        }

        private RatingsByBeerQuery.User GetUser(string id)
        {
            var user = QueryDispatcher.DispatchAsync<UserQuery, UserQuery.User>(new UserQuery { Id = id }).Result;

            return new RatingsByBeerQuery.User
            {
                Id = user.Id,
                AvatarUrl = user.AvatarUrl,
                Username = user.Username
            };
        }
    }
}
