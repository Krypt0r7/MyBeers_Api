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

namespace MyBeers.RatingLib.Api.QueryHandlers
{
    public class RatingsBasicsQueryHandler : BaseQueryHandler<Domain.Rating, RatingsBasicsQuery, IEnumerable<RatingsBasicsQuery.Rating>>
    {
        public RatingsBasicsQueryHandler(IMongoRepository<Rating> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override IEnumerable<RatingsBasicsQuery.Rating> Handle(RatingsBasicsQuery query)
        {
            var ratings = Repository.FilterBy(filter => true);

            return ratings.Select(x => new RatingsBasicsQuery.Rating
            {
                AfterTaste = x.AfterTaste,
                BeerId = x.BeerId,
                Chugability = x.Chugability,
                Description = x.Description,
                FirstImpression = x.FirstImpression,
                Id = x.Id.ToString(),
                OverallRating = x.OverallRating,
                Taste = x.Taste,
                UserId = x.UserId,
                Value = x.Value
            });

        }
    }
}
