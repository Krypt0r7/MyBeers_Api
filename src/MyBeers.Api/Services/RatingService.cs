using MongoDB.Driver;
using MyBeers.Api.Data;
using MyBeers.Api.Dtos;
using MyBeers.Api.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public class RatingService : IRatingService
    {
        private readonly IMongoCollection<Rating> _rating;
        public RatingService(IDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _rating = database.GetCollection<Rating>(settings.RatingCollection);
        }

        public async Task CreateRatingAsync(CreateRatingCommand ratingDto)
        {
            var rating = new Rating 
            {
                BeerId = ratingDto.Beer.Id,
                CreatedTime = DateTime.UtcNow,
                OverallRating = CalculateOVerallRating(ratingDto.AfterTaste, ratingDto.FirstImpression, ratingDto.Taste, ratingDto.Value),
                AfterTaste = ratingDto.AfterTaste,
                Chugability = ratingDto.Chugability,
                FirstImpression = ratingDto.FirstImpression,
                Taste = ratingDto.Taste,
                Value = ratingDto.Value,
                Description = ratingDto.Description,
                UserId = ratingDto.UserId
            };

            await _rating.InsertOneAsync(rating);
        }

        private double CalculateOVerallRating(int afterTaste, int firstImpression, int taste, int value)
        {
            
            double test = ((double)afterTaste + (double)firstImpression + (double)value + (double)taste) / 4;
            return test;
        }


        public async Task<Rating> GetRatingAsync(string id) => await _rating.Find(f => f.Id == id).FirstOrDefaultAsync();

        public async Task<List<Rating>> GetRatingsAsync(List<string> beerIds) =>  await _rating.Find(f => beerIds.Contains(f.BeerId)).ToListAsync();

        public async Task<List<Rating>> GetRatingsAsync() => await _rating.Find(f => true).Limit(20).ToListAsync();

        public async Task<List<Rating>> GetRatingsByUserId(string userId) => await _rating.Find(f => f.UserId == userId).ToListAsync();
        
        public async Task<UpdateResult> UpdateRatingAsync(string id, UpdateRatingCommand ratingCommand)
        {

            var filter = Builders<Rating>.Filter.Eq(f => f.Id, id);

            var oldRating = await _rating.Find(filter).FirstOrDefaultAsync();

            var update = Builders<Rating>.Update
                .Set(s => s.OverallRating, CalculateOVerallRating(ratingCommand.AfterTaste, ratingCommand.FirstImpression, ratingCommand.Taste, ratingCommand.Value))
                .Set(s => s.Description, ratingCommand.Description)
                .Set(s => s.Value, ratingCommand.Value)
                .Set(s => s.Taste, ratingCommand.Taste)
                .Set(s => s.FirstImpression, ratingCommand.FirstImpression)
                .Set(s => s.Chugability, ratingCommand.Chugability)
                .Set(s => s.AfterTaste, ratingCommand.AfterTaste);
            var result = await _rating.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false });
            return result;
        }
    }
}
