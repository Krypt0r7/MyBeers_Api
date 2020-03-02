using AutoMapper;
using MongoDB.Driver;
using MyBeers.Api.Data;
using MyBeers.Api.Dtos;
using MyBeers.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public class RatingService : IRatingService
    {
        private readonly IMongoCollection<Rating> _rating;
        private readonly IMapper _mapper;
        public RatingService(IDBSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _rating = database.GetCollection<Rating>(settings.RatingCollection);
            _mapper = mapper;
        }

        public async Task CreateRatingAsync(CreateRatingCommand ratingDto)
        {
            var rating = new Rating 
            {
                BeerId = ratingDto.Beer.Id,
                CreatedTime = DateTime.UtcNow,
                OverallRating = ratingDto.OverallRating,
                Description = ratingDto.Description,
                UserId = ratingDto.UserId
            };

            await _rating.InsertOneAsync(rating);
        }

        public async Task<Rating> GetRatingAsync(string id) => await _rating.Find(f => f.Id == id).FirstOrDefaultAsync();

        public async Task<List<Rating>> GetRatingsAsync(List<string> beerIds) =>  await _rating.Find(f => beerIds.Contains(f.BeerId)).ToListAsync();

        public async Task<List<Rating>> GetRatingsAsync() => await _rating.Find(f => true).ToListAsync();

        public async Task<List<Rating>> GetRatingsByUserId(string userId) => await _rating.Find(f => f.UserId == userId).ToListAsync();
        
        public async Task<UpdateResult> UpdateRatingAsync(string id, int rating, string description)
        {

            var filter = Builders<Rating>.Filter.Eq(f => f.Id, id);

            var oldRating = await _rating.Find(filter).FirstOrDefaultAsync();

            var update = Builders<Rating>.Update
                .Set(s => s.OverallRating, rating)
                .Set(s => s.Description, description);
            var result = await _rating.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = false });
            return result;
        }
    }
}
