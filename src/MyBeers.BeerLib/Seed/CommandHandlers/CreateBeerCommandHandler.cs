using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System.Threading.Tasks;
using System;
using MyBeers.BeerLib.Api.Commands;
using System.Linq;


namespace MyBeers.BeerLib.Seed.CommandHandlers
{
    public class CreateBeerCommandHandler : BaseCommandHandler<CreateBeerCommand, Domain.OldBeer>
    {
        public CreateBeerCommandHandler(IMongoRepository<OldBeer> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(CreateBeerCommand command)
        {
            var productIds = command.Containers.Select(x => x.ProductIdSystemet).ToList();
            var beer = await Repository.FilterByAsync(filter => productIds.Contains(filter.MoreInformation.ProductId));

            if (beer.Count() > 0)
                return;

            var newBeer = new OldBeer
            {
                AlcoholPercentage = command.AlcoholPercentage,
                City = command.City,
                Containers = command.Containers.Select(c => new OdlContainer
                {
                    Price = c.Price,
                    ProductionScale = c.ProductionScale,
                    RecycleFee = c.RecycleFee,
                    Type = c.Type,
                    Volume = c.Volume,
                    Ypk = c.YPK,
                    ProductIdFromSystmet = c.ProductIdSystemet,
                    SellStartDate = command.SellStartSystemet
                }),
                Country = command.Country,
                ImageUrl = command.ImageUrl,
                Name = command.Name,
                Producer = command.Producer,
                State = command.State,
                Style = command.Style,
                Type = command.Type,
                MoreInformation = new MoreInformation
                {
                    BeverageDescriptionShort = command.SystemetInformationModel.BeverageDescriptionShort,
                    AssortmentText = command.SystemetInformationModel.AssortmentText,
                    Assortment = command.SystemetInformationModel.Assortment,
                    AlcoholPercentage = command.SystemetInformationModel.AlcoholPercentage,
                    Category = command.SystemetInformationModel.Category,
                    Country = command.SystemetInformationModel.Country,
                    IsNews = command.SystemetInformationModel.IsNews,
                    IsOrganic = command.SystemetInformationModel.IsOrganic,
                    OriginLevel1 = command.SystemetInformationModel.OriginLevel1,
                    OriginLevel2 = command.SystemetInformationModel.OriginLevel2,
                    ProducerName = command.SystemetInformationModel.ProducerName,
                    ProductId = command.SystemetInformationModel.ProductId,
                    ProductNameBold = command.SystemetInformationModel.ProductNameBold,
                    ProductNameThin = command.SystemetInformationModel.ProductNameThin,
                    ProductNumber = command.SystemetInformationModel.ProductNumber,
                    ProductNumberShort = command.SystemetInformationModel.ProductNumberShort,
                    Style = command.SystemetInformationModel.Style,
                    SubCategory = command.SystemetInformationModel.SubCategory,
                    SupplierName = command.SystemetInformationModel.SupplierName,
                    Taste = command.SystemetInformationModel.Taste,
                    Type = command.SystemetInformationModel.Type,
                    Usage = command.SystemetInformationModel.Usage,
                    Vintage = command.SystemetInformationModel.Vintage,
                },
            };

            await Repository.SaveAsync(newBeer);

            Console.WriteLine(newBeer.Name + " was added");
        }
    }
}
