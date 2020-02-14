using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MyBeers.Api.Data;
using MyBeers.Api.DataSystemet;
using MyBeers.Api.Dtos;
using MyBeers.Api.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public class BeerService : IBeerService
    {
        private readonly IMongoCollection<Beer> _beer;
        private readonly IMapper _mapper;
        public BeerService(
            IMapper mapper,
            IMongoSettings mongoSettings)
        {
            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);
            _beer = database.GetCollection<Beer>(mongoSettings.BeerCollection);
            _mapper = mapper;
        }

        public async Task<Beer> SaveBeerAsync(SystemetDto systemetDto, string userId)
        {
            var beerData = _mapper.Map<BeerData>(systemetDto);

            var beer = new Beer
            {
                Added = DateTime.UtcNow,
                UserId = userId,
                YPK = Math.Round(beerData.Volume * beerData.AlcoholPercentage / 40 / beerData.Price, 3),
                BeerData = beerData
            };

            await _beer.InsertOneAsync(beer);

            return beer;
        }

        public async Task<List<Beer>> GetBeersAsync(string userId)
        {
            var beers = await _beer.Find(f => f.UserId == userId).ToListAsync();
            return beers;
        }

        public async Task<Beer> UpdateFavouriteAsync(string id)
        {
            var filter = Builders<Beer>.Filter.Eq(f => f.Id, id);
            var beer = await _beer.Find(filter).FirstOrDefaultAsync();
            if (beer == null)
                return null;

            var update = Builders<Beer>.Update
                .Set(s => s.Favourite, beer.Favourite ? false : true);

            await _beer.UpdateOneAsync(filter, update);

            return await _beer.Find(filter).FirstOrDefaultAsync();
        }


        public async Task<DeleteResult> DeleteAsync(string id)
        {
            var filter = Builders<Beer>.Filter.Eq(f => f.Id, id);
            var beer = await _beer.Find(filter).FirstOrDefaultAsync();
            if (beer == null)
                return null;

            var result = await _beer.DeleteOneAsync(filter);
            return result;
        }

        public async Task<Beer> SaveBeerProdNumberAsync(int productNumber, string userId)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://api-extern.systembolaget.se/")
            };

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "ce26456ac64b43a38dbc40dc6925177c");

            var response = await client.GetAsync($"product/v1/product/search?SearchQuery={productNumber}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var objects = JsonConvert.DeserializeObject<BeerListModel>(data);

                var mapppedBeer = _mapper.Map<BeerData>(objects.Hits[0]);

                var beer = new Beer
                {
                    Added = DateTime.UtcNow,
                    UserId = userId,
                    YPK = Math.Round(mapppedBeer.Volume * mapppedBeer.AlcoholPercentage / 40 / mapppedBeer.Price, 3),
                    BeerData = mapppedBeer
                };

                await _beer.InsertOneAsync(beer);

                return beer;
            }
            return null;

        }
    }
}
