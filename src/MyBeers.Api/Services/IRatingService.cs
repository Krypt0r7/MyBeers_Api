using MyBeers.Api.Data;
using MyBeers.Api.Dtos;
using System.Threading.Tasks;

namespace MyBeers.Api.Services
{
    public interface IRatingService
    {
        Task CreateRatingAsync(CreateRatingCommand ratingDto);
    }
}