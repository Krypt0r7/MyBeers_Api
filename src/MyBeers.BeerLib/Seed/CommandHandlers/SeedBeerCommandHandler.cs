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
                model,
                model.SellStartDate,
                null
                );

            return beerCommand;

        }
    }
}
