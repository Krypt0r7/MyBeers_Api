using MongoDB.Driver;
using MyBeers.Api.Data;
using MyBeers.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface IRatingService
    {
        Task CreateRatingAsync(CreateRatingCommand ratingDto);
        Task<UpdateResult> UpdateRatingAsync(string id, int rating);
        Task<Rating> GetRatingByIdAsync(string id);
        Task<List<Rating>> GetRatingsByUserId(string userId);
    }
}