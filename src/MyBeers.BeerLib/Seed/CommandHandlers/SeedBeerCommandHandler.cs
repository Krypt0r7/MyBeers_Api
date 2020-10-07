using Microsoft.Extensions.Options;
using MyBeers.BeerLib.Api.Commands;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace MyBeers.BeerLib.Seed.CommandHandlers
{
    public class SeedBeerCommandHandler : BaseCommandHandler<SeedBeerCommand, Domain.Beer>
    {
        private readonly HttpClient HttpClient = new HttpClient();
        private readonly AppSettings _appSettings;

        public SeedBeerCommandHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IOptions<AppSettings> options) : base(repository, queryDispatcher, commandDispatcher)
        {
            _appSettings = options.Value;
            HttpClient.BaseAddress = new Uri("https://api-extern.systembolaget.se/");
            HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.OcpKeySystemet);
        }

        public override async Task HandleAsync(SeedBeerCommand command)
        {
            var response = await HttpClient.GetAsync("product/v1/product");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var systemetObjects = JsonConvert.DeserializeObject<IEnumerable<SystemetInformationModel>>(data);
                var onlyBeers = systemetObjects.Where(x => x.Category == "Öl");

                var beerCommands = CreateBeer(onlyBeers);

                foreach (var beerCom in beerCommands)
                {
                    await CommandDispatcher.DispatchAsync(beerCom);
                }
                
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }


        private List<CreateBeerCommand> CreateBeer(IEnumerable<SystemetInformationModel> singleBeers)
        {
            var groupedBeers = singleBeers.GroupBy(x => x.ProductNumberShort);
            var beers = new List<CreateBeerCommand>();
            foreach (var group in groupedBeers)
            {   
                var containerList = new List<CreateBeerCommand.Container>();
                foreach (var beer in group)
                {
                    var container = new CreateBeerCommand.Container();
                    container.YPK = Math.Round(beer.Volume * beer.AlcoholPercentage / 40 / beer.Price, 3);
                    container.Price = beer.Price;
                    container.ProductionScale = beer.AssortmentText;
                    container.RecycleFee = beer.RecycleFee;
                    container.Type = beer.BottleTextShort;
                    container.Volume = beer.Volume;
                    container.ProductIdSystemet = beer.ProductId;

                    containerList.Add(container);
                }
                
                var singleBeer = group.First();

                string name = "";
                if (singleBeer.ProducerName != null && singleBeer.ProducerName.Contains(singleBeer.ProductNameBold))
                {

                    if (singleBeer.ProductNameThin == null)
                        name = singleBeer.ProductNameBold;
                    else
                        name = singleBeer.ProductNameThin;

                }
                else
                {
                    name = singleBeer.ProductNameBold + " " + singleBeer.ProductNameThin;
                }
                
                beers.Add(new CreateBeerCommand
                {
                    Style = singleBeer.Style,
                    Name = name,
                    Producer = singleBeer.ProducerName,
                    Containers = containerList,
                    AlcoholPercentage = singleBeer.AlcoholPercentage,
                    Country = singleBeer.Country,
                    State = singleBeer.OriginLevel1,
                    City = singleBeer.OriginLevel2,
                    Type = singleBeer.Type,
                    Usage = singleBeer.Usage,
                    Taste = singleBeer.Taste,
                    SystemetInformationModel = new SystemetInformationModel
                    {
                        BeverageDescriptionShort = singleBeer.BeverageDescriptionShort,
                        AssortmentText = singleBeer.AssortmentText,
                        Assortment = singleBeer.Assortment,
                        AlcoholPercentage = singleBeer.AlcoholPercentage,
                        BottleTextShort = singleBeer.BottleTextShort,
                        Category = singleBeer.Category,
                        Country = singleBeer.Country,
                        EthicalLabel = singleBeer.EthicalLabel,
                        IsCompletelyOutOfStock = singleBeer.IsCompletelyOutOfStock,
                        IsEthical = singleBeer.IsEthical,
                        IsKosher = singleBeer.IsKosher,
                        IsManufacturingCountry = singleBeer.IsManufacturingCountry,
                        IsNews = singleBeer.IsNews,
                        IsOrganic = singleBeer.IsOrganic,
                        IsRegionalRestricted = singleBeer.IsRegionalRestricted,
                        IsTemporaryOutOfStock = singleBeer.IsTemporaryOutOfStock,
                        IsWebLaunch = singleBeer.IsWebLaunch,
                        OriginLevel1 = singleBeer.OriginLevel1,
                        OriginLevel2 = singleBeer.OriginLevel2,
                        Price = singleBeer.Price,
                        ProducerName = singleBeer.ProducerName,
                        ProductId = singleBeer.ProductId,
                        ProductNameBold = singleBeer.ProductNameBold,
                        ProductNameThin = singleBeer.ProductNameThin,
                        ProductNumber = singleBeer.ProductNumber,
                        ProductNumberShort = singleBeer.ProductNumberShort,
                        RecycleFee = singleBeer.RecycleFee,
                        RestrictedParcelQuantity = singleBeer.RestrictedParcelQuantity,
                        Seal = singleBeer.Seal,
                        SellStartDate = singleBeer.SellStartDate,
                        Style = singleBeer.Style,
                        SubCategory = singleBeer.SubCategory,
                        SupplierName = singleBeer.SupplierName,
                        Taste = singleBeer.Taste,
                        Type = singleBeer.Type,
                        Usage = singleBeer.Usage,
                        Vintage = singleBeer.Vintage,
                        Volume  = singleBeer.Volume
                    },
                    SellStartSystemet = singleBeer.SellStartDate,
                    ImageUrl = null
                });
                
            }
            return beers;
        }
    }
}
