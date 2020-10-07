using MongoDB.Driver;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Migration.Api.Commands;
using MyBeers.RatingLib.Api.Commands;
using MyBeers.UserLib;
using MyBeers.UserLib.Api.Commands;
using MyBeers.UserLib.Api.Queries;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Migration.CommandHandlers
{
    public class MigrateDataCommandHandler : BaseCommandHandler<MigrateDataCommand, BeerLib.Domain.Beer>
    {
        private readonly MongoClient _mongoClient;
        public MigrateDataCommandHandler(IDBSettings settings,IMongoRepository<BeerLib.Domain.Beer> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
            _mongoClient = new MongoClient(settings.ConnectionString);
        }

        public override async Task HandleAsync(MigrateDataCommand command)
        {
            var beers = _mongoClient.GetDatabase("MyBeers").GetCollection<DataModels.Beer>("Beer").Find(x => true).ToList();
            var userCollection = _mongoClient.GetDatabase("MyBeers").GetCollection<DataModels.User>("User");
            var ratingsCollection = _mongoClient.GetDatabase("MyBeers").GetCollection<DataModels.Rating>("Rating");

            if (command.Users)
            {
                var users = userCollection.Find(f => true).ToList();

                foreach (var user in users)
                {
                    await CommandDispatcher.DispatchAsync(new CreateUserCommand
                    {
                        Email = user.Email,
                        Username = user.Username,
                        OldId = user.Id,
                        Password = "new-password",
                        Role = Roles.User
                    });

                }
            }
            if (command.Ratings)
            {
                var ratings = ratingsCollection.Find(f => true).ToList();

                foreach (var rating in ratings)
                {
                    var oldBeer = beers.First(x => x.Id == rating.BeerId);
                    var beer = await Repository.FindOneAsync(filter => filter.Containers.First().ProductIdFromSystmet == oldBeer.BeerData.ProductId);
                    var user = await QueryDispatcher.DispatchAsync<UserByOldIdQuery, UserByOldIdQuery.User>(new UserByOldIdQuery { OldId = rating.UserId });
                    await CommandDispatcher.DispatchAsync(new CreateUpdateRatingCommand
                    {
                        AfterTaste = rating.AfterTaste,
                        BeerId = beer.Id.ToString(),
                        Chugability = rating.Chugability,
                        Description = rating.Description,
                        FirstImpression = rating.FirstImpression,
                        Taste = rating.Taste,
                        Value = rating.Value
                    });
                }

            }
        }
    }
}
