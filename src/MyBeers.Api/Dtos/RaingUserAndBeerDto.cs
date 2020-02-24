using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class RaingUserAndBeerDto
    {
        public DateTime CreatedTime { get; set; }
        public int OverallRating { get; set; }
        public UserDto User { get; set; }
        public BeerDto Beer { get; set; }
    }
}
