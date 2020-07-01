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
                var systemetObjects = JsonConvert.DeserializeObject<IEnumerable<SystemetInformation>>(data);
                var onlyBeers = systemetObjects.Where(x => x.Category == "Öl");

                foreach (var beer in onlyBeers)
                {
                    await CommandDispatcher.DispatchAsync(CreateBeer(beer));
                }

            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }


        private CreateBeerCommand CreateBeer(SystemetInformation model)
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


            var beerCommand = new CreateBeerCommand
            {
                Volume = model.Volume,
                Style = model.Style,
                Name = name,
                Producer = model.ProducerName,
                Container = model.BottleTextShort,
                RecycleFee = model.RecycleFee,
                AlcoholPercentage = model.AlcoholPercentage,
                Price = model.Price,
                YPK = ypk,
                Country = model.Country,
                State = model.OriginLevel1,
                City = model.OriginLevel2,
                Type = model.Type,
                ProductionScale = model.AssortmentText,
                Usage = model.Usage,
                Taste = model.Taste,
                ProductIdSystemet = model.ProductId,
                SystemetInformationModel = new SystemetInformationModel
                {
                    AlcoholPercentage = model.AlcoholPercentage,
                    Assortment = model.Assortment,
                    AssortmentText = model.AssortmentText,
                    BeverageDescriptionShort = model.BeverageDescriptionShort
                },
                SellStartSystemet = model.SellStartDate,
                ImageUrl = null
            };

            return beerCommand;

        }
    }
}
