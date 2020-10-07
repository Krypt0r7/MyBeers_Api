using MongoDB.Bson;
using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Common.Services;
using MyBeers.RatingLib.Api.Commands;
using MyBeers.RatingLib.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.RatingLib.CommandHandlers
{
    public class CreateUpdateRatingCommandHandler : BaseCommandHandler<CreateUpdateRatingCommand, Domain.Rating>
    {
        private readonly IUserService userService;

        public CreateUpdateRatingCommandHandler(IMongoRepository<Rating> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IUserService userService) : base(repository, queryDispatcher, commandDispatcher)
        {
            this.userService = userService;
        }

        public override async Task HandleAsync(CreateUpdateRatingCommand command)
        {
            string userId = userService.GetUserId();
            var beer = await QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = command.BeerId });
            var ratingOld = await Repository.FilterByAsync(filter => filter.BeerId == command.BeerId && filter.UserId == userId);
            var rating = ratingOld.FirstOrDefault();
            if (rating == null)
            {
                var newRating = new Domain.Rating
                {
                    BeerId = beer.Id,
                    AfterTaste = command.AfterTaste,
                    Chugability = command.Chugability,
                    Description = command.Description,
                    Taste = command.Taste,
                    Value = command.Value,
                    FirstImpression = command.FirstImpression,
                    OverallRating = CalculateOverallRating(command.AfterTaste, command.FirstImpression, command.Taste, command.Value),
                    UserId = userId
                };

                await Repository.SaveAsync(newRating);
            }
            else
            {
                rating.AfterTaste = command.AfterTaste;
                rating.Chugability = command.Chugability;
                rating.Description = command.Description;
                rating.FirstImpression = command.FirstImpression;
                rating.Taste = command.Taste;
                rating.Value = command.Value;
                rating.OverallRating = CalculateOverallRating(command.AfterTaste, command.FirstImpression, command.Taste, command.Value);
                await Repository.ReplaceAsync(rating);
            }
        }

        private double CalculateOverallRating(int afterTaste, int firstImpression, int taste, int value)
        {
            return ((double)afterTaste + (double)firstImpression + (double)taste + (double)value) / 4;
        }
    }
}
