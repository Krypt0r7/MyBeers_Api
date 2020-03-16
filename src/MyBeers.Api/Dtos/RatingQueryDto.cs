using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class RatingQueryDto
    {
        public double OverallRating { get; set; }
        public int Taste { get; set; }
        public int AfterTaste { get; set; }
        public int Chugability { get; set; }
        public int Value { get; set; }
        public int FirstImpression { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
