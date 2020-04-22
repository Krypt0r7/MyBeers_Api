using MyBeers.Api.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface ISystemetService
    {
        Task<List<Beer.BeerDataModel>> SearchSystemetAsync(string searchString);
        Task<Beer.BeerDataModel> SearchSingleBeer(int id);
        Task<List<Beer.BeerDataModel>> GetNews(string region);
    }
}