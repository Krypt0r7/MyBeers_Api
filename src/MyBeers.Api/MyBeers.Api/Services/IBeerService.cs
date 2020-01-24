using MongoDB.Driver;
using MyBeers.Api.Data;
using MyBeers.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface IBeerService
    {
        Task<DeleteResult> DeleteAsync(string id);
        Task<List<Beer>> GetBeersAsync(string userId);
        Task<Beer> SaveBeerAsync(SystemetDto systemetDto, string userId);
        Task<Beer> UpdateFavouriteAsync(string id);
    }
}