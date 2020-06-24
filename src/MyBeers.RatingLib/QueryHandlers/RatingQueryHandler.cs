using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.RatingLib.Domain;
using MyBeers.RatingLib.Api.Queries;
using System.Threading.Tasks;
using MyBeers.UserLib.Api.Queries;
using MyBeers.BeerLib.Api.Queries;

namespace MyBeers.RatingLib.QueryHandlers
{
    public class RatingQueryHandler : BaseQueryHandler<Domain.Rating, RatingQuery, RatingQuery.Rating>
    {
        public RatingQueryHandler(IMongoRepository<Rating> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override RatingQuery.Rating Handle(RatingQuery query)
        {
            var rating = Repository.FindById(query.Id);
            var user = QueryDispatcher.Dispatch<UserQuery, UserQuery.User>(new UserQuery { Id = rating.UserId });
            var beer = QueryDispatcher.Dispatch<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = rating.BeerId });

            return new RatingQuery.Rating
            {
                Id = rating.Id.ToString(),
                AfterTaste = rating.AfterTaste,
                Chugability = rating.Chugability,
                Description = rating.Description,
                FirstImpression = rating.FirstImpression,
                OverallRating = rating.OverallRating,
                Taste = rating.Taste,
                Value = rating.Value,
                User = new RatingQuery.User
                {
                    AvatarUrl = user.AvatarUrl,
                    Id = user.Id,
                    Username = user.Username
                },
                Beer = new RatingQuery.Beer
                {
                    Id = beer.Id,
                    Alcohol = beer.AlcoholPercentage,
                    Name = beer.Name,
                    Price = beer.Price,
                    Producer = beer.Producer,
                    Volume = beer.Volume
                }
            };
        }
    }
}
