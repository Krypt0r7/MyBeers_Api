using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.RatingLib.Api.Commands;
using MyBeers.RatingLib.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.RatingLib.CommandHandlers
{
    public class CreateUpdateRatingCommandHandler : BaseCommandHandler<CreateUpdateRatingCommand, Domain.Rating>
    {
        public CreateUpdateRatingCommandHandler(IMongoRepository<Rating> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(CreateUpdateRatingCommand command)
        {
            var beer = await QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = command.BeerId });

            var rating = Repository.FilterBy(filter => filter.BeerId == command.BeerId && filter.UserId == command.UserId).FirstOrDefault();

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
                UserId = command.UserId
            };

            if (rating == null)
                await Repository.SaveAsync(newRating);
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
