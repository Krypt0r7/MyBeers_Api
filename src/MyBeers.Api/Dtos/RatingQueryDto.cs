using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class RatingQueryDto
    {
        public int OverallRating { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
