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
        public RatingService(IMongoSettings settings, IMapper mapper)
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
                UserId = ratingDto.User.Id
            };

            await _rating.InsertOneAsync(rating);
        }
    }
}
