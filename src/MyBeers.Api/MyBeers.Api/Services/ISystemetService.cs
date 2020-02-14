using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface ISystemetService
    {
        Task<string> SearchSystemetAsync(string searchString);
    }
}