using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.RatingLib.Api.Queries;
using MyBeers.RatingLib.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.RatingLib.QueryHandlers
{
    public class RatingsByUserQueryHandler : BaseQueryHandler<Domain.Rating, RatingsByUserQuery, IEnumerable<RatingsByUserQuery.Rating>>
    {
        public RatingsByUserQueryHandler(IMongoRepository<Rating> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<RatingsByUserQuery.Rating>> HandleAsync(RatingsByUserQuery query)
        {
            var ratings = await Task.Run(() => Repository.FilterByAsync(filter => filter.UserId == query.UserId));

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
