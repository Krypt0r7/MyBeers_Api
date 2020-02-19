using MongoDB.Driver;
using MyBeers.Api.Data;
using MyBeers.Api.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface IBeerService
    {
        Task<Beer> SaveBeerProdNumberAsync(int productNumber);
        Task<List<Beer>> GetUsersBeerAsync(User user);
        Task<Beer> GetBeerByIdAsync(string id);
    }
}