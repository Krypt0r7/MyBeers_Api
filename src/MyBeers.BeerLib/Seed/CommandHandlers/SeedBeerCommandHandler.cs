using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyBeers.BeerLib.Domain;
using MyBeers.BeerLib.Seed.Commands;
using MyBeers.Common.Bases;
using MyBeers.Common.CommonInterfaces;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
                var systemetObjects = JsonConvert.DeserializeObject<IEnumerable<SystemetModel>>(data);
                var onlyBeers = systemetObjects.Where(x => x.Category == "Öl");

                foreach (var beer in onlyBeers)
                {
                    await CommandDispatcher.DispatchAsync(CreateBeer(beer));
                }

            }
        }


        private CreateBeerCommand CreateBeer(SystemetModel model)
        {
            string name = "";
            if (model.ProducerName != null && model.ProducerName.Contains(model.ProductNameBold))
            {

                if (model.ProductNameThin == null)
                    name = model.ProductNameBold;
                else
                    name = model.ProductNameThin;

            }
            else
            {
                name = model.ProductNameBold + " " + model.ProductNameThin;
            }

            double ypk = Math.Round(model.Volume * model.AlcoholPercentage / 40 / model.Price, 3);


            var beerCommand = new CreateBeerCommand(
                name,
                model.ProducerName,
                model.BottleTextShort,
                model.RecycleFee,
                model.AlcoholPercentage,
                model.Volume,
                model.Price,
                ypk,
                model.Country,
                model.OriginLevel1,
                model.OriginLevel2,
                model.Type,
                model.Style,
                model.AssortmentText,
                model.Usage,
                model.Taste,
                model.ProductId,
                model.SellStartDate,
                null
                );

            return beerCommand;

        }


        private class SystemetModel
        {
            public long ProductId { get; set; }
            public long ProductNumber { get; set; }
            public string ProductNameBold { get; set; }
            public string ProductNameThin { get; set; }
            public string Category { get; set; }
            public long ProductNumberShort { get; set; }
            public string ProducerName { get; set; }
            public string SupplierName { get; set; }
            public bool IsKosher { get; set; }
            public string BottleTextShort { get; set; }
            public string Seal { get; set; }
            public long RestrictedParcelQuantity { get; set; }
            public bool IsOrganic { get; set; }
            public bool IsEthical { get; set; }
            public string EthicalLabel { get; set; }
            public bool IsWebLaunch { get; set; }
            public DateTime SellStartDate { get; set; }
            public bool IsCompletelyOutOfStock { get; set; }
            public bool IsTemporaryOutOfStock { get; set; }
            public double AlcoholPercentage { get; set; }
            public double Volume { get; set; }
            public double Price { get; set; }
            public string Country { get; set; }
            public string OriginLevel1 { get; set; }
            public string OriginLevel2 { get; set; }
            public int Vintage { get; set; }
            public string SubCategory { get; set; }
            public string Type { get; set; }
            public string Style { get; set; }
            public string AssortmentText { get; set; }
            public string BeverageDescriptionShort { get; set; }
            public string Usage { get; set; }
            public string Taste { get; set; }
            public string Assortment { get; set; }
            public double RecycleFee { get; set; }
            public bool IsManufacturingCountry { get; set; }
            public bool IsRegionalRestricted { get; set; }
            public bool IsNews { get; set; }
        }
    }
}
