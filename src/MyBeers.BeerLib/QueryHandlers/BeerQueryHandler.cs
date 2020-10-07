using MyBeers.BeerLib.Domain;
using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace MyBeers.BeerLib.beerHandlers
{
    public class BeerQueryHandler : BaseQueryHandler<Beer, BeerQuery, BeerQuery.Beer>
    {
        public BeerQueryHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<BeerQuery.Beer> HandleAsync(BeerQuery query)
        {
            var beer = await Repository.FindByIdAsync(query.Id);

            if (beer == null)
                throw new Exception("Beer not found");

            return new BeerQuery.Beer
            {
                Id = beer.Id.ToString(),
                AlcoholPercentage = beer.AlcoholPercentage,
                City = beer.City,
                Country = beer.Country,
                ImageUrl = beer.ImageUrl,
                Name = beer.Name,
                Producer = beer.Producer,
                State = beer.State,
                Style = beer.Style,
                Type = beer.Type,
                Containers = beer.Containers.Select(c => new BeerQuery.Beer.Container{
                    Price = c.Price,
                    ProductIdFromSystemet = c.ProductIdFromSystmet,
                    ProductionScale = c.ProductionScale,
                    RecycleFee = c.RecycleFee,
                    SellStartDate = c.SellStartDate,
                    Type = c.Type,
                    Volume = c.Volume,
                    Ypk = c.Ypk
                }),
                MoreInformationModel = new BeerQuery.Beer.MoreInformation{
                    BeverageDescriptionShort = beer.MoreInformation.BeverageDescriptionShort,
                    AssortmentText = beer.MoreInformation.AssortmentText,
                    Assortment = beer.MoreInformation.Assortment,
                    AlcoholPercentage = beer.MoreInformation.AlcoholPercentage,
                    Category = beer.MoreInformation.Category,
                    Country = beer.MoreInformation.Country,
                    IsNews = beer.MoreInformation.IsNews,
                    IsOrganic = beer.MoreInformation.IsOrganic,
                    OriginLevel1 = beer.MoreInformation.OriginLevel1,
                    OriginLevel2 = beer.MoreInformation.OriginLevel2,
                    ProducerName = beer.MoreInformation.ProducerName,
                    ProductId = beer.MoreInformation.ProductId,
                    ProductNameBold = beer.MoreInformation.ProductNameBold,
                    ProductNameThin = beer.MoreInformation.ProductNameThin,
                    ProductNumber = beer.MoreInformation.ProductNumber,
                    ProductNumberShort = beer.MoreInformation.ProductNumberShort,
                    Style = beer.MoreInformation.Style,
                    SubCategory = beer.MoreInformation.SubCategory,
                    SupplierName = beer.MoreInformation.SupplierName,
                    Taste = beer.MoreInformation.Taste,
                    Type = beer.MoreInformation.Type,
                    Usage = beer.MoreInformation.Usage,
                    Vintage = beer.MoreInformation.Vintage,
                }
            };
        }
    }
}
