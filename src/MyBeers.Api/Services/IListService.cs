using MongoDB.Driver;
using MyBeers.Api.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface IListService
    {
        Task CreateListAsync(string userId);
        Task<List<List>> GetListAsync();
        Task<List<List>> GetUsersListAsync(string userId);
        Task<List> GetListAsync(string id);
        Task<UpdateResult> UpdateListAsync(string id);
        Task<DeleteResult> DeleteListAsync(string id);
    }
}
