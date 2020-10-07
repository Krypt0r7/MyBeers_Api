using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Common.Services;
using MyBeers.RatingLib.Api.Queries;
using MyBeers.UserLib.Api.Queries;
using MyBeers.UserLib.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.UserLib.QueryHandlers
{
    public class UserAllDataQueryHandler : BaseQueryHandler<Domain.User, UserAllDataQuery, UserAllDataQuery.User>
    {
        private readonly IUserService userService;

        public UserAllDataQueryHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher, IUserService userService) : base(repository, queryDispatcher)
        {
            this.userService = userService;
        }

        public override async Task<UserAllDataQuery.User> HandleAsync(UserAllDataQuery query)
        {
            var user = await Repository.FindByIdAsync(userService.GetUserId());

            var ratings = await QueryDispatcher.DispatchAsync<RatingsByUserQuery, IEnumerable<RatingsByUserQuery.Rating>>(new RatingsByUserQuery());

            return new UserAllDataQuery.User
            {
                Email = user.Email,
                Id = user.Id.ToString(),
                Username = user.Username,
                Ratings = ratings.Select(x => new UserAllDataQuery.Rating
                {
                    AfterTaste = x.AfterTaste,
                    Chugability = x.Chugability,
                    Value = x.Value,
                    Description = x.Description,
                    FirstImpression = x.FirstImpression,
                    OverallRating = x.OverallRating,
                    Taste = x.Taste,
                    Beer = GetBeer(x.BeerId).Result
                })
            };
        }

        private async Task<UserAllDataQuery.Beer> GetBeer(string beerId)
        {
            var beer = await QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = beerId });
            return new UserAllDataQuery.Beer
            {
                Id = beer.Id,
                Alcohol = beer.AlcoholPercentage,
                Name = beer.Name,
                Producer = beer.Producer,
            };
        }
    }
}
