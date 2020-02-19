using MyBeers.Api.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface ISystemetService
    {
        Task<List<BeerData>> SearchSystemetAsync(string searchString);
        Task<BeerData> SearchSingleBeer(int id);
    }
}