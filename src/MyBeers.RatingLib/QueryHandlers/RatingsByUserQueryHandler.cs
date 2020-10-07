using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Common.Services;
using MyBeers.RatingLib.Api.Queries;
using MyBeers.RatingLib.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.RatingLib.QueryHandlers
{
    public class RatingsByUserQueryHandler : BaseQueryHandler<Domain.Rating, RatingsByUserQuery, IEnumerable<RatingsByUserQuery.Rating>>
    {
        private readonly IUserService userService;

        public RatingsByUserQueryHandler(IMongoRepository<Rating> repository, IQueryDispatcher queryDispatcher, IUserService userService) : base(repository, queryDispatcher)
        {
            this.userService = userService;
        }

        public override async Task<IEnumerable<RatingsByUserQuery.Rating>> HandleAsync(RatingsByUserQuery query)
        {
            string userId = userService.GetUserId();
            var ratings = await Task.Run(() => Repository.FilterByAsync(filter => filter.UserId == userId));

            return ratings.Select(x => new RatingsByUserQuery.Rating
            {
                AfterTaste = x.AfterTaste,
                UserId = x.UserId,
                BeerId = x.BeerId,
                Chugability = x.Chugability,
                Description = x.Description,
                FirstImpression = x.FirstImpression,
                Id = x.Id.ToString(),
                OverallRating = x.OverallRating,
                Taste = x.Taste,
                Value = x.Value
            });
        }
    }
}
