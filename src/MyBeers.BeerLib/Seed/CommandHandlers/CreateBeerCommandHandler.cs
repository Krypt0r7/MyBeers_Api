using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System.Threading.Tasks;
using System;
using MyBeers.BeerLib.Api.Commands;

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
                SystemetInformation = new SystemetInformation
                {
                    BeverageDescriptionShort = command.SystemetInformationModel.BeverageDescriptionShort,
                    AssortmentText = command.SystemetInformationModel.AssortmentText,
                    Assortment = command.SystemetInformationModel.Assortment,
                    AlcoholPercentage = command.SystemetInformationModel.AlcoholPercentage,
                    BottleTextShort = command.SystemetInformationModel.BottleTextShort,
                    Category = command.SystemetInformationModel.Category,
                    Country = command.SystemetInformationModel.Country,
                    EthicalLabel = command.SystemetInformationModel.EthicalLabel,
                    IsCompletelyOutOfStock = command.SystemetInformationModel.IsCompletelyOutOfStock,
                    IsEthical = command.SystemetInformationModel.IsEthical,
                    IsKosher = command.SystemetInformationModel.IsKosher,
                    IsManufacturingCountry = command.SystemetInformationModel.IsManufacturingCountry,
                    IsNews = command.SystemetInformationModel.IsNews,
                    IsOrganic = command.SystemetInformationModel.IsOrganic,
                    IsRegionalRestricted = command.SystemetInformationModel.IsRegionalRestricted,
                    IsTemporaryOutOfStock = command.SystemetInformationModel.IsTemporaryOutOfStock,
                    IsWebLaunch = command.SystemetInformationModel.IsWebLaunch,
                    OriginLevel1 = command.SystemetInformationModel.OriginLevel1,
                    OriginLevel2 = command.SystemetInformationModel.OriginLevel2,
                    Price = command.SystemetInformationModel.Price,
                    ProducerName = command.SystemetInformationModel.ProducerName,
                    ProductId = command.SystemetInformationModel.ProductId,
                    ProductNameBold = command.SystemetInformationModel.ProductNameBold,
                    ProductNameThin = command.SystemetInformationModel.ProductNameThin,
                    ProductNumber = command.SystemetInformationModel.ProductNumber,
                    ProductNumberShort = command.SystemetInformationModel.ProductNumberShort,
                    RecycleFee = command.SystemetInformationModel.RecycleFee,
                    RestrictedParcelQuantity = command.SystemetInformationModel.RestrictedParcelQuantity,
                    Seal = command.SystemetInformationModel.Seal,
                    SellStartDate = command.SystemetInformationModel.SellStartDate,
                    Style = command.SystemetInformationModel.Style,
                    SubCategory = command.SystemetInformationModel.SubCategory,
                    SupplierName = command.SystemetInformationModel.SupplierName,
                    Taste = command.SystemetInformationModel.Taste,
                    Type = command.SystemetInformationModel.Type,
                    Usage = command.SystemetInformationModel.Usage,
                    Vintage = command.SystemetInformationModel.Vintage,
                    Volume  = command.SystemetInformationModel.Volume
                },
                YPK = command.YPK,
                SellStartDate = command.SellStartSystemet
            };

            await Repository.SaveAsync(newBeer);

            Console.WriteLine(newBeer.Name + " was added");
        }
    }
}
