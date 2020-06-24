using MyBeers.BeerLib.Seed.Commands;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System.Threading.Tasks;
using System;

namespace MyBeers.BeerLib.Seed.CommandHandlers
{
    public class CreateBeerCommandHandler : BaseCommandHandler<CreateBeerCommand, Domain.Beer>
    {
        public CreateBeerCommandHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(CreateBeerCommand command)
        {
            var beer = Repository.FindOne(filter => filter.ProductIdSystemet == command.ProductIdSystemet);

            if (beer != null)
                return;

            var newBeer = new Beer
            {
                AlcoholPercentage = command.AlcoholPercentage,
                City = command.City,
                Container = command.Container,
                Country = command.Country,
                ImageUrl = command.ImageUrl,
                Name = command.Name,
                Price = command.Price,
                Producer = command.Producer,
                ProductIdSystemet = command.ProductIdSystemet,
                ProductionScale = command.ProductionScale,
                RecycleFee = command.RecycleFee,
                State = command.State,
                Style = command.Style,
                Taste = command.Taste,
                Type = command.Type,
                Usage = command.Usage,
                Volume = command.Volume,
                SystemetInformation = command.SystemetInformation,
                YPK = command.YPK,
                SellStartDate = command.SellStartSystemet
            };

            await Repository.SaveAsync(newBeer);

            Console.WriteLine(newBeer.Name + " was added");
        }
    }
}
