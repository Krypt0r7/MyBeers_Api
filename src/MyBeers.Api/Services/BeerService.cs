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
        private readonly IRatingService _ratingService;
        private readonly ISystemetService _systemetService;
        public BeerService(
            IDBSettings mongoSettings,
            ISystemetService systemetService,
            IRatingService ratingService)
        {
            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);
            _beer = database.GetCollection<Beer>(mongoSettings.BeerCollection);
            _systemetService = systemetService;
            _ratingService = ratingService;
        }


        public async Task<Beer> SaveBeerProdNumberAsync(int productId)
        {
            var beerExisting = await _beer.Find(x => x.BeerData.ProductId == productId).FirstOrDefaultAsync();

            if (beerExisting != null)
                return beerExisting;

            var mapppedBeer = await _systemetService.SearchSingleBeer(productId);
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

            return await _beer.Find(x => x.BeerData.ProductId == productId).FirstOrDefaultAsync();
        }

        public async Task<List<Beer>> GetUsersBeerAsync(List<string> beerIds)
        {
            return await _beer.Find(f => beerIds.Contains(f.Id)).SortByDescending(s => s.Added).ToListAsync();
        }

        public async Task<Beer> GetBeerByIdAsync(string id)
        {
            var beer = await _beer.Find(f => f.Id == id).FirstOrDefaultAsync();
            if (beer == null)
                return null;
            return beer;
        }

        public async Task<List<Beer>> GetAllBeersAsync() =>  await _beer.Find(f => true).ToListAsync();

        public async Task<List<BeerAverageRatingDto>> GetTopOrBottomRatedBeerAsync(string userId = null, bool top = true)
        {
            var ratings = userId == null ? await _ratingService.GetRatingsAsync() : await _ratingService.GetRatingsByUserId(userId);
            var groupedRatings = ratings.GroupBy(rating => rating.BeerId).ToList();
            var collection = new Dictionary<string, double>();
            foreach (var beer in groupedRatings)
            {
                double average = 0;
                foreach (var rating in beer)
                {
                    average += rating.OverallRating;
                }
                average /= beer.Count();
                collection.Add(beer.Key, average);
            }

            var orderedCollection = top ? collection.OrderByDescending(o => o.Value).Take(5) : collection.OrderBy(o => o.Value).Take(5);

            var ids = orderedCollection.Select(x => x.Key).ToList();

            var beers = await _beer.Find(f => ids.Contains(f.Id)).ToListAsync();

            var listOfBeer = new List<BeerAverageRatingDto>();

            foreach (var keyValue in orderedCollection)
            {
                var theBeer = beers.FirstOrDefault(x => x.Id == keyValue.Key); 
                listOfBeer.Add(new BeerAverageRatingDto
                {
                    Added = theBeer.Added,
                    Id = theBeer.Id,
                    AverageRating = keyValue.Value,
                    BeerData = theBeer.BeerData,
                    YPK = theBeer.YPK
                });
            }

            return listOfBeer;
        }
    }
}
