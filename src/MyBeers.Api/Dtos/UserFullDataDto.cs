using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class UserFullDataDto : UserDto
    {
        public List<RatingBeerDto> Ratings { get; set; }
        public List<BeerAverageRatingDto> BestRatedBeer { get; set; }

        public List<BeerAverageRatingDto> WorstRatedBeer { get; set; }
    }
}
