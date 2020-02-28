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
        private readonly ISystemetService _systemetService;
        public BeerService(
            IDBSettings mongoSettings,
            ISystemetService systemetService)
        {
            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);
            _beer = database.GetCollection<Beer>(mongoSettings.BeerCollection);
            _systemetService = systemetService;
        }


        public async Task<Beer> SaveBeerProdNumberAsync(int productNumber)
        {
            var beerExisting = await _beer.Find(x => x.BeerData.ProductNumber == productNumber).FirstOrDefaultAsync();

            if (beerExisting != null)
                return beerExisting;

            var mapppedBeer = await _systemetService.SearchSingleBeer(productNumber);
            if (mapppedBeer == null)
                return null;

            mapppedBeer.ImageUrl = BuildImageUrls.BuildUrl((int)mapppedBeer.ProductId);

            var beer = new Beer
            {
                Added = DateTime.UtcNow,
                YPK = Math.Round(mapppedBeer.Volume * mapppedBeer.AlcoholPercentage / 40 / mapppedBeer.Price, 3),
                BeerData = mapppedBeer
            };

            await _beer.InsertOneAsync(beer);

            return await _beer.Find(x => x.BeerData.ProductNumber == productNumber).FirstOrDefaultAsync();
        }

        public async Task<List<Beer>> GetUsersBeerAsync(List<string> beerIds)
        {
            return await _beer.Find(f => beerIds.Contains(f.Id)).ToListAsync();
        }

        public async Task<Beer> GetBeerByIdAsync(string id)
        {
            var beer = await _beer.Find(f => f.Id == id).FirstOrDefaultAsync();
            if (beer == null)
                return null;
            return beer;
        }

        public async Task<List<Beer>> GetAllBeersAsync() =>  await _beer.Find(f => true).ToListAsync();
     
    }
}
