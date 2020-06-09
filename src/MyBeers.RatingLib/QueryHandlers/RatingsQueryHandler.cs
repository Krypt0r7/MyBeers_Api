﻿using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.RatingLib.Domain;
using MyBeers.RatingLib.Api.Queries;
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
            var ratings = await Task.Run(() => Repository.AsQueryable().ToList().OrderByDescending(o => o.Created).Take(30));

            var users = ratings.GroupBy(g => g.UserId)
                .Select(z => QueryDispatcher.DispatchAsync<UserQuery, UserQuery.User>(new UserQuery { Id = z.Key }).Result)
                .Select(x => new RatingsQuery.User { Id = x.Id, AvatarUrl = x.AvatarUrl, Username = x.Username });

            var beers = ratings.GroupBy(g => g.BeerId)
                .Select(z => QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = z.Key }).Result)
                .Select(x => new RatingsQuery.Beer { Id = x.Id, Alcohol = x.AlcoholPercentage, Name = x.Name, Price = x.Price, Producer = x.Producer, Volume = x.Volume });

            return ratings.Select(x => new RatingsQuery.Rating 
            {
                AfterTaste = x.AfterTaste,
                Chugability = x.Chugability,
                Description = x.Description,
                FirstImpression = x.FirstImpression,
                OverallRating = x.OverallRating,
                Taste = x.Taste,
                Value = x.Value,
                Id = x.Id.ToString(),
                User = users.FirstOrDefault(y => x.UserId == y.Id),
                Beer = beers.FirstOrDefault(y => x.BeerId == y.Id)
            });

        }
    }
}