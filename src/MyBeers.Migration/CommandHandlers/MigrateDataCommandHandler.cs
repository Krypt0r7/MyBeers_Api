using MongoDB.Driver;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Migration.Api.Commands;
using MyBeers.RatingLib.Api.Commands;
using MyBeers.UserLib.Api.Commands;
using MyBeers.UserLib.Api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    await CommandDispatcher.DispatchAsync(new CreateUserCommand(user.Username, "new-password", user.Email, user.Id));

                }
            }
            if (command.Ratings)
            {
                var ratings = ratingsCollection.Find(f => true).ToList();

                foreach (var rating in ratings)
                {
                    var oldBeer = beers.First(x => x.Id == rating.BeerId);
                    var beer = Repository.FindOne(filter => filter.ProductIdSystemet == oldBeer.BeerData.ProductId);
                    var user = QueryDispatcher.Dispatch<UserByOldIdQuery, UserByOldIdQuery.User>(new UserByOldIdQuery { OldId = rating.UserId });
                    await CommandDispatcher.DispatchAsync(new CreateUpdateRatingCommand(
                        rating.Taste, 
                        rating.AfterTaste, 
                        rating.Chugability, 
                        rating.Value, 
                        rating.FirstImpression, 
                        rating.Description, 
                        beer.Id.ToString(), 
                        user.Id));
                }

            }
        }
    }
}
