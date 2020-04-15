using MongoDB.Driver;
using MyBeers.Api.Data;
using MyBeers.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface IBeerService
    {
        Task<Beer> SaveBeerProdNumberAsync(int productId);
        Task<List<Beer>> GetUsersBeerAsync(List<string> beerIds);
        Task<Beer> GetBeerByIdAsync(string id);
        Task<List<BeerAverageRatingDto>> GetTopOrBottomRatedBeerAsync(string userId = null, bool top = true);
        Task<List<Beer>> GetAllBeersAsync();
    }
}