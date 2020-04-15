using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class RatingBeerDto : RatingQueryDto
    {
        public BeerDto Beer { get; set; }
    }
}
